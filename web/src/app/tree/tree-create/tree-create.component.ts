import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TreeCreateRequest } from '../shared/tree-create-request.model';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-tree-create',
  templateUrl: './tree-create.component.html',
  styleUrls: ['./tree-create.component.scss']
})
export class TreeCreateComponent implements OnInit {
  @Output() submitted = new EventEmitter();
  model: TreeCreateRequest = { treeName: null };

  constructor(
    private service: TreeService,
  ) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.service.createTree(this.model).subscribe(() => {
      this.submitted.emit();
      this.model.treeName = '';
    });
  }

}
