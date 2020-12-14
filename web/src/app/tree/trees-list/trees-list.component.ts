import { Component, OnInit } from '@angular/core';
import { TreesListElement } from '../shared/trees-list-element.model';
import { TreeService } from '../shared/tree.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-trees-list',
  templateUrl: './trees-list.component.html',
  styleUrls: ['./trees-list.component.scss']
})
export class TreesListComponent implements OnInit {
  treesList: TreesListElement[] = [];

  constructor(
    private service: TreeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.onModelChange();
  }

  onModelChange() {
    this.service.getMyTrees().subscribe(result => {
      this.treesList = result.trees;
    });
  }

  navigateToTree(tree: TreesListElement) {
    this.router.navigate(['/tree', tree.id]);
  }
}
