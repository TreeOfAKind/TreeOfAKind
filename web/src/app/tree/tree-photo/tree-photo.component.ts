import { Component, Input, OnInit } from '@angular/core';
import { TreeService } from '../shared/tree.service';

@Component({
  selector: 'app-tree-photo',
  templateUrl: './tree-photo.component.html',
  styleUrls: ['./tree-photo.component.scss']
})
export class TreePhotoComponent implements OnInit {
  @Input() treeId: string;

  constructor(
    private service: TreeService
  ) { }

  ngOnInit(): void {
  }

  changePhoto(files: FileList) {
    const formData: FormData = new FormData();
    formData.append('treeId', this.treeId);
    formData.append('image', files.item(0));

    this.service.changePhoto(formData).subscribe();
  }
}
