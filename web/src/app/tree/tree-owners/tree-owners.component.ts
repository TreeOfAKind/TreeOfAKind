import { Component, Input, OnInit } from '@angular/core';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-tree-owners',
  templateUrl: './tree-owners.component.html',
  styleUrls: ['./tree-owners.component.scss']
})
export class TreeOwnersComponent implements OnInit {
  @Input() treeId: string;
  email: string;

  constructor(
    private service: TreeService
  ) { }

  ngOnInit(): void {
  }

  addOwner() {
    this.service.addOwner(this.treeId, this.email).subscribe(); //TODO: reload owners list
  }

  leaveTree() {
    if (confirm('Are you sure to leave this tree?')) {
      this.service.removeOwner(this.treeId).subscribe();
    }
  }
}
