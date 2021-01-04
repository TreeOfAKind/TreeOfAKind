import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PersonResponse } from 'src/app/people/shared/person-response.model';
import { TreeService } from 'src/app/tree/shared/tree.service';

@Component({
  selector: 'app-poster',
  templateUrl: './poster.component.html',
  styleUrls: ['./poster.component.scss']
})
export class PosterComponent implements OnInit {
  treeId: string;
  people: PersonResponse[];
  allChecked: boolean = true;
  checked: boolean[];
  checkedPeople: PersonResponse[];

  constructor(
    private service: TreeService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.parent.snapshot.paramMap.get('id');
    this.service.getTree(this.treeId).subscribe(tree => {
      this.people = this.checkedPeople = tree.people;
      this.checked = new Array(this.people.length);
      this.checkAll();
    });
  }

  checkAll() {
    for(let i = 0; i < this.checked.length; ++i) {
      this.checked[i] = this.allChecked;
    }
  }

  refreshPicture() {
    this.checkedPeople = [];
    for(let i = 0; i < this.people.length; ++i) {
      if(this.checked[i]) {
        this.checkedPeople.push(this.people[i]);
      }
    }
  }

}
