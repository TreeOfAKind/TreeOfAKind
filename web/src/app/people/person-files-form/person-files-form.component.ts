import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TreeService } from 'src/app/tree/shared/tree.service';
import { PeopleService } from '../shared/people.service';

@Component({
  selector: 'app-person-files-form',
  templateUrl: './person-files-form.component.html',
  styleUrls: ['./person-files-form.component.scss']
})
export class PersonFilesFormComponent implements OnInit {
  treeId: string;
  personId: string;
  mainPhotoUrl: string;

  constructor(
    private service: PeopleService,
    private treeService: TreeService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.parent.snapshot.paramMap.get('id');
    this.personId = this.route.snapshot.paramMap.get('id');

    this.treeService.getTree(this.treeId).subscribe(tree => {
      const people = tree.people.filter(person => person.id !== this.personId);

      const person = tree.people.find(per => per.id == this.personId);
      if(person.mainPhoto != null) {
        this.mainPhotoUrl = person.mainPhoto.uri;
      }
    });
  }

  uploadPhoto(files: File[]) {
    const formData: FormData = new FormData();
    formData.append('treeId', this.treeId);
    formData.append('personId', this.personId);
    formData.append('file', files[0]);

    this.service.changePersonsMainPhoto(formData).subscribe(response => {
      this.mainPhotoUrl = response.uri;
    });
  }
}
