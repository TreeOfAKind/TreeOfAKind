import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormAction } from 'src/app/helpers/form-action.enum';
import { TreeService } from 'src/app/tree/shared/tree.service';
import { Gender } from '../shared/gender.enum';
import { PeopleService } from '../shared/people.service';
import { PersonForm } from '../shared/person-form.model';

@Component({
  selector: 'app-person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.scss']
})
export class PersonFormComponent implements OnInit {
  model: PersonForm = {
    id: null,
    treeId: null,
    name: null,
    lastName: null,
    gender: "Unknown",
    birthDate: null,
    deathDate: null,
    description: null,
    biography: null,
    mother: null,
    father: null,
    spouse: null
  };
  Gender = Gender;
  keys = [];
  treeId: string;
  personId: string;
  people: PersonForm[];
  formAction: FormAction;
  FormAction = FormAction;

  constructor(
    private service: PeopleService,
    private treeService: TreeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.keys = Object.keys(Gender);
    this.treeId = this.route.parent.snapshot.paramMap.get('id');
    this.model.treeId = this.treeId;
    this.formAction = this.route.snapshot.data["formAction"];

    this.personId = this.route.snapshot.paramMap.get('id');
    this.treeService.getTree(this.treeId).subscribe(tree => {
      this.people = tree.people.filter(person => person.id !== this.personId);

      if (this.formAction == FormAction.Edit) {
        const person = tree.people.find(per => per.id == this.personId);
        this.model = {
          id: person.id,
          treeId: person.treeId,
          name: person.name,
          lastName: person.lastName,
          gender: person.gender,
          birthDate: person.birthDate,
          deathDate: person.deathDate,
          description: person.description,
          biography: person.biography,
          mother: person.mother,
          father: person.father,
          spouse: person.spouse
        };
      }
    });
  }

  onSubmit() {
    this.service.addPerson(this.model).subscribe(res => {
      if (res != null) {
        this.router.navigate([`/tree/${this.treeId}`]);
      }
    });
  }

}
