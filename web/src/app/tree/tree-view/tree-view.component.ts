import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss']
})
export class TreeViewComponent implements OnInit {
  treeId: string;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.snapshot.paramMap.get('id');
  }

}
