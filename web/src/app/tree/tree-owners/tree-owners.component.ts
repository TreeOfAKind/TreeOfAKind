import { Component, Input, OnInit } from '@angular/core';
import { Owner } from '../shared/owner.model';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-tree-owners',
  templateUrl: './tree-owners.component.html',
  styleUrls: ['./tree-owners.component.scss']
})
export class TreeOwnersComponent implements OnInit {
  @Input() treeId: string;
  email: string;
  owners: Owner[];

  constructor(
    private service: TreeService
  ) { }

  ngOnInit(): void {
    this.loadModel();
  }

  addOwner() {
    this.service.addOwner(this.treeId, this.email).subscribe(() => {
      this.loadModel();
    });
  }

  leaveTree() {
    if (confirm('Are you sure to leave this tree?')) {
      this.service.removeOwner(this.treeId).subscribe(() => {
        this.loadModel();
      });
    }
  }

  deleteOwner(owner: Owner) {
    if (confirm('Are you sure to remove this owner?')) {
      this.service.removeOwner(this.treeId, owner.id).subscribe(() => {
        this.loadModel();
      });
    }
  }

  private loadModel() {
    this.service.getTree(this.treeId).subscribe(tree => {
      this.owners = tree.owners;
    });
  }
}
