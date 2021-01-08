import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PersonResponse } from 'src/app/people/shared/person-response.model';
import { TreeDiagramService } from '../shared/tree-diagram.service';

@Component({
  selector: 'app-tree-draw',
  templateUrl: './tree-draw.component.html',
  styleUrls: ['./tree-draw.component.scss']
})
export class TreeDrawComponent implements OnInit, OnChanges {
  @Input() people: PersonResponse[];

  constructor(
    private service: TreeDiagramService
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (this.people?.length > 0) {
      this.service.drawDiagram(this.people);
    }
  }

  ngOnInit() {
    if (this.people?.length > 0) {
      this.service.drawDiagram(this.people);
    }
  }

}
