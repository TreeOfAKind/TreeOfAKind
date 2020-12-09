import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { TreeService } from '../shared/tree.service';

import { TreeCreateComponent } from './tree-create.component';

describe('TreeCreateComponent', () => {
  let component: TreeCreateComponent;
  let fixture: ComponentFixture<TreeCreateComponent>;
  let treeService: TreeService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TreeCreateComponent ],
      imports: [
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
      ],
      providers: [
        {
          provide: TreeService, useValue: {
            createTree: () => of({
              id: "idValue"
            })
          }
        },
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TreeCreateComponent);
    component = fixture.componentInstance;
    treeService = fixture.debugElement.injector.get(TreeService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call service create method', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      const button = fixture.debugElement.query(By.css('button[name="createTreeButton"]')).nativeElement;
      const onSubmitSpy = spyOn(component, 'onSubmit').and.callThrough();
      const updateSpy = spyOn(treeService, 'createTree').and.returnValue(of({
        id: 'idValue'
      }));

      button.click();

      expect(onSubmitSpy).toHaveBeenCalled();
      expect(updateSpy).toHaveBeenCalled();
    })
  }));

  it('should emit submitted event', waitForAsync(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      const button = fixture.debugElement.query(By.css('button[name="createTreeButton"]')).nativeElement;
      const submittedSpy = spyOn(component.submitted, 'emit');

      button.click();

      expect(submittedSpy).toHaveBeenCalled();
    })
  }));
});
