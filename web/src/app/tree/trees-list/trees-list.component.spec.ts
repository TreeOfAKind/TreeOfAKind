import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { TreeService } from '../shared/tree.service';
import { TreesListResponse } from '../shared/trees-list-response.model';

import { TreesListComponent } from './trees-list.component';

describe('TreesListComponent', () => {
  let component: TreesListComponent;
  let fixture: ComponentFixture<TreesListComponent>;
  let trees: TreesListResponse = {
    trees: [
      {
        treeId: "1",
        treeName: "Tree 0",
        photoUri: null
      },
      {
        treeId: "2",
        treeName: "Tree 1",
        photoUri: null
      }
    ]
  }

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TreesListComponent ],
      imports: [
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
      ],
      providers: [
        {
          provide: TreeService, useValue: {
            getMyTrees: () => of(trees),
          }
        },
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TreesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display proper number of trees', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      expect(fixture.componentInstance.treesList.length).toBe(trees.trees.length);
    });
  }));
});
