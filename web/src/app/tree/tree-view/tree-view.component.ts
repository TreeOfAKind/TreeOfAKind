import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Tree } from '../shared/tree.model';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss']
})
export class TreeViewComponent implements OnInit {
  model: Tree = {
    treeId: null,
    treeName: null,
    photoUri: null,
    people: []
  };
  treeId: string;
  expanded: string[];

  constructor(
    private service: TreeService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.snapshot.paramMap.get('id');
    this.loadModel();
  }

  expand(index: number) {
    this.expanded = new Array(3);
    this.expanded[index] = "show";
  }

  private loadModel() {
    this.service.getTree(this.treeId).subscribe(result => {
      this.model = result;
    });
  }

}
