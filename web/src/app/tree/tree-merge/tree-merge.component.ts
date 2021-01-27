import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TreeService } from '../shared/tree.service';
import { TreesListElement } from '../shared/trees-list-element.model';

@Component({
  selector: 'app-tree-merge',
  templateUrl: './tree-merge.component.html',
  styleUrls: ['./tree-merge.component.scss']
})
export class TreeMergeComponent implements OnInit {
  model = {
    firstTreeId: null,
    secondTreeId: null
  };
  trees: TreesListElement[];

  constructor(
    private service: TreeService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.service.getMyTrees().subscribe(result => {
      this.trees = result.trees;
    })
  }

  onSubmit() {
    this.service.mergeTrees(this.model.firstTreeId, this.model.secondTreeId)
      .subscribe(res => {
        if (res) {
          this.router.navigate(['']);
        }
      });
  }
}
