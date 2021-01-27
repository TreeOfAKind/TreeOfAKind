import { Component, Input, OnInit } from '@angular/core';
import { TreeService } from 'src/app/tree/shared/tree.service';
import { PeopleService } from '../shared/people.service';
import { PersonForm } from '../shared/person-form.model';

@Component({
  selector: 'app-people-list',
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.scss']
})
export class PeopleListComponent implements OnInit {
  @Input() treeId: string;
  model: PersonForm[];

  constructor(
    private service: PeopleService,
    private treeService: TreeService
  ) { }

  ngOnInit(): void {
    this.loadModel();
  }

  deletePerson(person: PersonForm) {
    this.service.removePerson(person.id, this.treeId).subscribe(result => {
      this.loadModel();
    });
  }

  private loadModel() {
    this.treeService.getTree(this.treeId).subscribe(result => {
      this.model = result.people;
    });
  }
}
