import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { UserProfileFormComponent } from './user-profile-form.component';
import { AppRoutingModule } from '../../app-routing.module';
import { UserProfileService } from '../shared/user-profile.service';
import { of } from 'rxjs';
import { UserProfile } from '../shared/user-profile.model';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';


describe('UserProfileFormComponent', () => {
  let component: UserProfileFormComponent;
  let fixture: ComponentFixture<UserProfileFormComponent>;
  let userProfileService: UserProfileService;
  let router: Router;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserProfileFormComponent ],
      imports: [
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
      ],
      providers: [
        {
          provide: UserProfileService, useValue: {
            getUserProfile: () => of({
              firstName: 'fName',
              lastName: 'lName',
              birthDate: new Date(2000, 12, 30)
            }),
            updateUserProfile: () => of ({
              id: "idValue"
            })
          }
        },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfileFormComponent);
    component = fixture.componentInstance;
    userProfileService = fixture.debugElement.injector.get(UserProfileService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display proper values', async(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      expect(fixture.componentInstance.model.firstName).toBe("fName");
      expect(fixture.componentInstance.model.lastName).toBe("lName");
      expect(fixture.componentInstance.model.birthDate).toEqual(new Date(2000, 12, 30));
    });
  }));

  it('should call service update method', async(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();

      const button = fixture.debugElement.query(By.css('button[name="submitUserProfileButton"]')).nativeElement;
      const onSubmitSpy = spyOn(component, 'onSubmit').and.callThrough();
      const updateSpy = spyOn(userProfileService, 'updateUserProfile').and.returnValue(of({
        id: 'idValue'
      }));
      const routerSpy = spyOn(router, 'navigate');

      button.click();

      expect(routerSpy).toHaveBeenCalled();
      expect(updateSpy).toHaveBeenCalled();
      expect(onSubmitSpy).toHaveBeenCalled();
    })
  }));
});
