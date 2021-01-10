import { Injectable } from '@angular/core';
import * as go from 'gojs';
import { PersonResponse } from 'src/app/people/shared/person-response.model';
import { DiagramMember } from './diagram-member.model';

@Injectable({
  providedIn: 'root'
})
export class TreeDiagramService {
  backgroundImage: string;

  constructor() { }

  drawDiagram(people: PersonResponse[]) {
    this.convertPeopleToDiagramMembers(people);
    init(this.backgroundImage);
   }

  downloadDiagram() {
    makeSvg();
  }

  changeBackgroundImage(image: string) {
    this.backgroundImage = image;
    init(this.backgroundImage);
  }

  private convertPeopleToDiagramMembers(people: PersonResponse[]) {
    diagramMembers = [];
    for (let i = 0; i < people.length; ++i) {
      let person = people[i];
      let diagramMember: DiagramMember = {
        key: i,
        n: `${person.name} ${person.lastName}`,
        s: 'M',
        m: person.mother && people.find(p => p.id === person.mother) ?
          people.findIndex(p => p.id === person.mother) : null,
        f: person.father && people.find(p => p.id === person.father) ?
          people.findIndex(p => p.id === person.father) : null,
        ux: person.spouse && people.find(p => p.id === person.spouse) ?
          people.findIndex(p => p.id === person.spouse) : null,
        photo: person.mainPhoto ? person.mainPhoto.uri : '/assets/person-black-18dp.svg'
      }
      diagramMembers.push(diagramMember);
    }
  }

}

var diagramMembers: DiagramMember[];

// gojs code

go.Diagram.licenseKey = "54f947ebba6031b700ca0d2b113f69ed1bb37a679dd41ef25e5741a6ef0968442bceed2858d78f93d0ac4ef81d74c7898e916d7bc41e0768e037db894bb183ade13776e0061841daf75326c5caf37da0fe7d71a2cbe772f2d3788aa5eca9c29d5abaf1dc1b9d0abc7d7f01325129a74de2a8dc7cad55c91d6b6a99f7aae8fc40ff3e71829ae051dffd49778befeba00c267105cf39fc7fa1043642c7dd5b";

var myDiagram;

function init(backgroundImage) {
  //if (window.goSamples) goSamples();  // init for these samples -- you don't need to call this
  var $ = go.GraphObject.make;
  if (myDiagram != null) {
    myDiagram.div = null;
  }
  myDiagram =
    $(go.Diagram, "myDiagramDiv",
      {
        initialAutoScale: go.Diagram.Uniform,
        "undoManager.isEnabled": true,
        // when a node is selected, draw a big yellow circle behind it
        nodeSelectionAdornmentTemplate:
          $(go.Adornment, "Auto",
            { layerName: "Grid" },  // the predefined layer that is behind everything else
            $(go.Shape, "Circle", { fill: "#c1cee3", stroke: null }),
            $(go.Placeholder, { margin: 2 })
          ),
        layout:  // use a custom layout, defined below
          // @ts-ignore
          $(GenogramLayout, { direction: 90, layerSpacing: 30, columnSpacing: 10 })
      });
  if (backgroundImage) {
    myDiagram.add(
      $(go.Part,  
        { layerName: "Background", position: new go.Point(0, 0),
          selectable: false, pickable: false },
        $(go.Picture, backgroundImage, { pickable: true })
      ));
  }

  // two different node templates, one for each sex,
  // named by the category value in the node data object
  myDiagram.nodeTemplateMap.add("M",  // male
    $(go.Node, "Vertical",
      { locationSpot: go.Spot.Center, locationObjectName: "ICON", selectionObjectName: "ICON" },
      $(go.Panel,
        { name: "ICON" },
        $(go.Picture, {width: 80, height: 100},
          new go.Binding("source", "photo")),
      ),
      $(go.TextBlock,
        { textAlign: "center", maxSize: new go.Size(80, NaN), margin: 5 },
        new go.Binding("text", "n"))
    ));

  // the representation of each label node -- nothing shows on a Marriage Link
  myDiagram.nodeTemplateMap.add("LinkLabel",
    $(go.Node, { selectable: false, width: 1, height: 1, fromEndSegmentLength: 20 }));


  myDiagram.linkTemplate =  // for parent-child relationships
    $(go.Link,
      {
        routing: go.Link.Orthogonal, corner: 5,
        layerName: "Background", selectable: false,
        fromSpot: go.Spot.Bottom, toSpot: go.Spot.Top
      },
      $(go.Shape, { stroke: "#424242", strokeWidth: 2 })
    );

  myDiagram.linkTemplateMap.add("Marriage",  // for marriage relationships
    $(go.Link,
      { selectable: false },
      $(go.Shape, { strokeWidth: 2.5, stroke: "#5d8cc1" /* blue */ })
    ));


  // n: name, s: sex, m: mother, f: father, ux: wife, vir: husband, a: attributes/markers
  setupDiagram(myDiagram, diagramMembers,
    diagramMembers[0].key /* focus on this person */);
}


// create and initialize the Diagram.model given an array of node data representing people
function setupDiagram(diagram, array, focusId) {
  diagram.model =
    go.GraphObject.make(go.GraphLinksModel,
      { // declare support for link label nodes
        linkLabelKeysProperty: "labelKeys",
        // this property determines which template is used
        nodeCategoryProperty: "s",
        // if a node data object is copied, copy its data.a Array
        copiesArrays: true,
        // create all of the nodes for people
        nodeDataArray: array
      });
  setupMarriages(diagram);
  setupParents(diagram);

  var node = diagram.findNodeForKey(focusId);
  if (node !== null) {
    diagram.select(node);
    // remove any spouse for the person under focus:
    //node.linksConnected.each(function(l) {
    //  if (!l.isLabeledLink) return;
    //  l.opacity = 0;
    //  var spouse = l.getOtherNode(node);
    //  spouse.opacity = 0;
    //  spouse.pickable = false;
    //});
  }
}

function findMarriage(diagram, a, b) {  // A and B are node keys
  var nodeA = diagram.findNodeForKey(a);
  var nodeB = diagram.findNodeForKey(b);
  if (nodeA !== null && nodeB !== null) {
    var it = nodeA.findLinksBetween(nodeB);  // in either direction
    while (it.next()) {
      var link = it.value;
      // Link.data.category === "Marriage" means it's a marriage relationship
      if (link.data !== null && link.data.category === "Marriage") return link;
    }
  }
  return null;
}

// now process the node data to determine marriages
function setupMarriages(diagram) {
  var model = diagram.model;
  var nodeDataArray = model.nodeDataArray;
  for (var i = 0; i < nodeDataArray.length; i++) {
    var data = nodeDataArray[i];
    var key = data.key;
    var uxs = data.ux;
    if (uxs != null) {
      if (typeof uxs === "number") uxs = [uxs];
      for (var j = 0; j < uxs.length; j++) {
        var wife = uxs[j];
        if (key === wife) {
          // or warn no reflexive marriages
          continue;
        }
        var link = findMarriage(diagram, key, wife);
        if (link === null) {
          // add a label node for the marriage link
          var mlab1 = { s: "LinkLabel" };
          model.addNodeData(mlab1);
          // add the marriage link itself, also referring to the label node
          // @ts-ignore
          var mdata = { from: key, to: wife, labelKeys: [mlab1.key], category: "Marriage" }; //TODO
          model.addLinkData(mdata);
        }
      }
    }
    var virs = data.vir;
    if (virs != null) {
      if (typeof virs === "number") virs = [virs];
      for (var j = 0; j < virs.length; j++) {
        var husband = virs[j];
        if (key === husband) {
          // or warn no reflexive marriages
          continue;
        }
        var link = findMarriage(diagram, key, husband);
        if (link === null) {
          // add a label node for the marriage link
          var mlab1 = { s: "LinkLabel" };
          model.addNodeData(mlab1);
          // add the marriage link itself, also referring to the label node
          // @ts-ignore
          var mdata = { from: key, to: husband, labelKeys: [mlab1.key], category: "Marriage" }; //TODO
          model.addLinkData(mdata);
        }
      }
    }
  }
}

// process parent-child relationships once all marriages are known
function setupParents(diagram) {
  var model = diagram.model;
  var nodeDataArray = model.nodeDataArray;
  for (var i = 0; i < nodeDataArray.length; i++) {
    var data = nodeDataArray[i];
    var key = data.key;
    var mother = data.m;
    var father = data.f;
    if (mother != null && father != null) {
      var link = findMarriage(diagram, mother, father);
      if (link === null) {
        // or warn no known mother or no known father or no known marriage between them
          var cdata = { from: mother, to: key };
          myDiagram.model.addLinkData(cdata);
          cdata = { from: father, to: key };
          myDiagram.model.addLinkData(cdata);
        continue;
      }
      var mdata = link.data;
      var mlabkey = mdata.labelKeys[0];
      var cdata = { from: mlabkey, to: key };
      myDiagram.model.addLinkData(cdata);
    }
    else if (mother != null) {
      var cdata = { from: mother, to: key };
      myDiagram.model.addLinkData(cdata);
    }
    else if (father != null) {
      var cdata = { from: father, to: key };
      myDiagram.model.addLinkData(cdata);
    }
  }
}


// A custom layout that shows the two families related to a person's parents
function GenogramLayout() {
  go.LayeredDigraphLayout.call(this);
  this.initializeOption = go.LayeredDigraphLayout.InitDepthFirstIn;
  this.spouseSpacing = 30;  // minimum space between spouses
}
go.Diagram.inherit(GenogramLayout, go.LayeredDigraphLayout);

GenogramLayout.prototype.makeNetwork = function(coll) {
  // generate LayoutEdges for each parent-child Link
  var net = this.createNetwork();
  if (coll instanceof go.Diagram) {
    this.add(net, coll.nodes, true);
    this.add(net, coll.links, true);
  } else if (coll instanceof go.Group) {
    this.add(net, coll.memberParts, false);
  } else if (coll.iterator) {
    this.add(net, coll.iterator, false);
  }
  return net;
};

// internal method for creating LayeredDigraphNetwork where husband/wife pairs are represented
// by a single LayeredDigraphVertex corresponding to the label Node on the marriage Link
GenogramLayout.prototype.add = function(net, coll, nonmemberonly) {
  var multiSpousePeople = new go.Set();
  // consider all Nodes in the given collection
  var it = coll.iterator;
  while (it.next()) {
    var node = it.value;
    if (!(node instanceof go.Node)) continue;
    if (!node.isLayoutPositioned || !node.isVisible()) continue;
    if (nonmemberonly && node.containingGroup !== null) continue;
    // if it's an unmarried Node, or if it's a Link Label Node, create a LayoutVertex for it
    if (node.isLinkLabel) {
      // get marriage Link
      var link = node.labeledLink;
      var spouseA = link.fromNode;
      var spouseB = link.toNode;
      // create vertex representing both husband and wife
      var vertex = net.addNode(node);
      // now define the vertex size to be big enough to hold both spouses
      vertex.width = spouseA.actualBounds.width + this.spouseSpacing + spouseB.actualBounds.width;
      vertex.height = Math.max(spouseA.actualBounds.height, spouseB.actualBounds.height);
      vertex.focus = new go.Point(spouseA.actualBounds.width + this.spouseSpacing / 2, vertex.height / 2);
    } else {
      // don't add a vertex for any married person!
      // instead, code above adds label node for marriage link
      // assume a marriage Link has a label Node
      var marriages = 0;
      node.linksConnected.each(function(l) { if (l.isLabeledLink) marriages++; });
      if (marriages === 0) {
        var vertex = net.addNode(node);
      } else if (marriages > 1) {
        multiSpousePeople.add(node);
      }
    }
  }
  // now do all Links
  it.reset();
  it = coll.iterator;
  while (it.next()) {
    var link1 = it.value;
    if (!(link1 instanceof go.Link)) continue;
    if (!link1.isLayoutPositioned || !link1.isVisible()) continue;
    if (nonmemberonly && link1.containingGroup !== null) continue;
    // if it's a parent-child link, add a LayoutEdge for it
    if (!link1.isLabeledLink) {
      var parent = net.findVertex(link1.fromNode);  // should be a label node
      var child = net.findVertex(link1.toNode);
      if (child !== null) {  // an unmarried child
        net.linkVertexes(parent, child, link1);
      } else {  // a married child
        link1.toNode.linksConnected.each(function(l) {
          if (!l.isLabeledLink) return;  // if it has no label node, it's a parent-child link
          // found the Marriage Link, now get its label Node
          var mlab = l.labelNodes.first();
          // parent-child link should connect with the label node,
          // so the LayoutEdge should connect with the LayoutVertex representing the label node
          var mlabvert = net.findVertex(mlab);
          if (mlabvert !== null) {
            net.linkVertexes(parent, mlabvert, link1);
          }
        });
      }
    }
  }

  while (multiSpousePeople.count > 0) {
    // find all collections of people that are indirectly married to each other
    var node1 = multiSpousePeople.first();
    var cohort = new go.Set();
    this.extendCohort(cohort, node1);
    // then encourage them all to be the same generation by connecting them all with a common vertex
    var dummyvert = net.createVertex();
    net.addVertex(dummyvert);
    var marriages1 = new go.Set();
    cohort.each(function(n: go.Node) {
      n.linksConnected.each(function(l) {
        marriages1.add(l);
      })
    });
    marriages1.each(function(link) {
      // find the vertex for the marriage link (i.e. for the label node)
      var mlab = link1.labelNodes.first()
      var v = net.findVertex(mlab);
      if (v !== null) {
        net.linkVertexes(dummyvert, v, null);
      }
    });
    // done with these people, now see if there are any other multiple-married people
    multiSpousePeople.removeAll(cohort);
  }
};

// collect all of the people indirectly married with a person
GenogramLayout.prototype.extendCohort = function(coll, node) {
  if (coll.has(node)) return;
  coll.add(node);
  var lay = this;
  node.linksConnected.each(function(l) {
    if (l.isLabeledLink) {  // if it's a marriage link, continue with both spouses
      lay.extendCohort(coll, l.fromNode);
      lay.extendCohort(coll, l.toNode);
    }
  });
};

GenogramLayout.prototype.assignLayers = function() {
  // @ts-ignore
  go.LayeredDigraphLayout.prototype.assignLayers.call(this);
  var horiz = this.direction == 0.0 || this.direction == 180.0;
  // for every vertex, record the maximum vertex width or height for the vertex's layer
  var maxsizes = [];
  this.network.vertexes.each(function(v) {
    var lay = v.layer;
    var max = maxsizes[lay];
    if (max === undefined) max = 0;
    var sz = (horiz ? v.width : v.height);
    if (sz > max) maxsizes[lay] = sz;
  });
  // now make sure every vertex has the maximum width or height according to which layer it is in,
  // and aligned on the left (if horizontal) or the top (if vertical)
  this.network.vertexes.each(function(v) {
    var lay = v.layer;
    var max = maxsizes[lay];
    if (horiz) {
      v.focus = new go.Point(0, v.height / 2);
      v.width = max;
    } else {
      v.focus = new go.Point(v.width / 2, 0);
      v.height = max;
    }
  });
  // from now on, the LayeredDigraphLayout will think that the Node is bigger than it really is
  // (other than the ones that are the widest or tallest in their respective layer).
};

GenogramLayout.prototype.commitNodes = function() {
  // @ts-ignore
  go.LayeredDigraphLayout.prototype.commitNodes.call(this);
  // position regular nodes
  this.network.vertexes.each(function(v) {
    if (v.node !== null && !v.node.isLinkLabel) {
      v.node.position = new go.Point(v.x, v.y);
    }
  });
  // position the spouses of each marriage vertex
  var layout = this;
  this.network.vertexes.each(function(v) {
    if (v.node === null) return;
    if (!v.node.isLinkLabel) return;
    var labnode = v.node;
    var lablink = labnode.labeledLink;
    // In case the spouses are not actually moved, we need to have the marriage link
    // position the label node, because LayoutVertex.commit() was called above on these vertexes.
    // Alternatively we could override LayoutVetex.commit to be a no-op for label node vertexes.
    lablink.invalidateRoute();
    var spouseA = lablink.fromNode;
    var spouseB = lablink.toNode;
    // prefer fathers on the left, mothers on the right
    if (spouseA.data.s === "F") {  // sex is female
      var temp = spouseA;
      spouseA = spouseB;
      spouseB = temp;
    }
    // see if the parents are on the desired sides, to avoid a link crossing
    var aParentsNode = layout.findParentsMarriageLabelNode(spouseA);
    var bParentsNode = layout.findParentsMarriageLabelNode(spouseB);
    if (aParentsNode !== null && bParentsNode !== null && aParentsNode.position.x > bParentsNode.position.x) {
      // swap the spouses
      var temp = spouseA;
      spouseA = spouseB;
      spouseB = temp;
    }
    spouseA.position = new go.Point(v.x, v.y);
    spouseB.position = new go.Point(v.x + spouseA.actualBounds.width + layout.spouseSpacing, v.y);
    if (spouseA.opacity === 0) {
      var pos = new go.Point(v.centerX - spouseA.actualBounds.width / 2, v.y);
      spouseA.position = pos;
      spouseB.position = pos;
    } else if (spouseB.opacity === 0) {
      var pos = new go.Point(v.centerX - spouseB.actualBounds.width / 2, v.y);
      spouseA.position = pos;
      spouseB.position = pos;
    }
  });
  // position only-child nodes to be under the marriage label node
  this.network.vertexes.each(function(v) {
    if (v.node === null || v.node.linksConnected.count > 1) return;
    var mnode = layout.findParentsMarriageLabelNode(v.node);
    if (mnode !== null && mnode.linksConnected.count === 1) {  // if only one child
      var mvert = layout.network.findVertex(mnode);
      var newbnds = v.node.actualBounds.copy();
      newbnds.x = mvert.centerX - v.node.actualBounds.width / 2;
      // see if there's any empty space at the horizontal mid-point in that layer
      var overlaps = layout.diagram.findObjectsIn(newbnds, function(x) { return x.part; }, function(p) { return p !== v.node; }, true);
      if (overlaps.count === 0) {
        v.node.move(newbnds.position);
      }
    }
  });
};

GenogramLayout.prototype.findParentsMarriageLabelNode = function(node) {
  var it = node.findNodesInto();
  while (it.next()) {
    var n = it.value;
    if (n.isLinkLabel) return n;
  }
  return null;
};
// end GenogramLayout class

// poster
function toDataURL(url, callback) {
  var xhr = new XMLHttpRequest();
  xhr.onload = function() {
    var reader = new FileReader();
    reader.onloadend = function() {
      callback(reader.result);
    }
    reader.readAsDataURL(xhr.response);
  };
  xhr.open('GET', url);
  xhr.responseType = 'blob';
  xhr.send();
}

function makeSvg() {
  var svg = myDiagram.makeSvg({
    scale: 1, background: "white",
    elementFinished: function(graphobject, svgelement) {
      if (!(graphobject instanceof go.Picture)) return;
      toDataURL(svgelement.href.baseVal, function(dataUrl) {
        svgelement.setAttribute('href', dataUrl);
      });
    }
  });
  var svgstr = new XMLSerializer().serializeToString(svg);
  var myWindow = window.open();
  myWindow.document.write(svgstr);
}
