import { Component, OnInit } from '@angular/core';
import { TreesListElement } from '../shared/trees-list-element.model';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-trees-list',
  templateUrl: './trees-list.component.html',
  styleUrls: ['./trees-list.component.scss']
})
export class TreesListComponent implements OnInit {
  treesList: TreesListElement[] = [];

  constructor(
    private service: TreeService
  ) { }

  ngOnInit(): void {
    this.service.getMyTrees().subscribe(result => {
      this.treesList = this.treesList.concat(result.trees);
    });
  }
}
