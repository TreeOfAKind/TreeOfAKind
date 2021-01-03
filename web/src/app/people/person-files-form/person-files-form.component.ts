import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TreeService } from 'src/app/tree/shared/tree.service';
import { FileResponse } from '../shared/file-response.model';
import { PeopleService } from '../shared/people.service';
import { RemovePersonsFileRequest } from '../shared/remove-persons-file.model';

@Component({
  selector: 'app-person-files-form',
  templateUrl: './person-files-form.component.html',
  styleUrls: ['./person-files-form.component.scss']
})
export class PersonFilesFormComponent implements OnInit {
  treeId: string;
  personId: string;
  mainPhotoUrl: string;
  files: FileResponse[];

  constructor(
    private service: PeopleService,
    private treeService: TreeService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.parent.snapshot.paramMap.get('id');
    this.personId = this.route.snapshot.paramMap.get('id');

    this.treeService.getTree(this.treeId).subscribe(tree => {
      const person = tree.people.find(per => per.id == this.personId);
      if(person.mainPhoto != null) {
        this.mainPhotoUrl = person.mainPhoto.uri;
      }
      this.files = person.files;
    });
  }

  uploadPhoto(files: File[]) {
    const formData = this.prepareFormData(files[0]);

    this.service.changePersonsMainPhoto(formData).subscribe(response => {
      this.mainPhotoUrl = response.uri;
    });
  }

  uploadFile(files: File[]) {
    const formData = this.prepareFormData(files[0]);

    this.service.addPersonsFile(formData).subscribe(res => {
      this.updateFilesList();
    });
  }

  deleteFile(file: FileResponse) {
    const request: RemovePersonsFileRequest = {
      treeId: this.treeId,
      personId: this.personId ,
      fileId: file.id
    }

    this.service.removePersonsFile(request).subscribe(res => {
      this.updateFilesList();
    })
  }

  private prepareFormData(file: File): FormData {
    const formData: FormData = new FormData();
    formData.append('treeId', this.treeId);
    formData.append('personId', this.personId);
    formData.append('file', file);

    return formData;
  }

  private updateFilesList() {
    this.treeService.getTree(this.treeId).subscribe(tree => {
      const person = tree.people.find(per => per.id == this.personId);
      this.files = person.files;
    });
  }
}
