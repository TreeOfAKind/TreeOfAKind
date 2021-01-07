import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { AngularFireModule } from "@angular/fire";
import { AngularFireAuthModule } from "@angular/fire/auth";
import { environment } from 'src/environments/environment';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './authentication/shared/token-interceptor';
import { UserProfileFormComponent } from './user-profile/user-profile-form/user-profile-form.component';
import { TreesListComponent } from './tree/trees-list/trees-list.component'
import { CommonModule } from '@angular/common';
import { TreeCreateComponent } from './tree/tree-create/tree-create.component';
import { TreeViewComponent } from './tree/tree-view/tree-view.component';
import { PersonFormComponent } from './people/person-form/person-form.component';
import { PersonPipe } from './people/shared/person.pipe';
import { TreePhotoComponent } from './tree/tree-photo/tree-photo.component';
import { PeopleListComponent } from './people/people-list/people-list.component';
import { TreeOwnersComponent } from './tree/tree-owners/tree-owners.component';
import { HttpErrorInterceptor } from './helpers/error-interceptor';
import { PersonFilesFormComponent } from './people/person-files-form/person-files-form.component';
import { TreeDrawComponent } from './tree/tree-draw/tree-draw.component';
import { LicensesComponent } from './licenses/licenses.component';
import { PosterComponent } from './import-export/poster/poster.component';
import { ImportComponent } from './import-export/import/import.component';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    UserProfileFormComponent,
    TreesListComponent,
    TreeCreateComponent,
    TreeViewComponent,
    PersonFormComponent,
    PersonPipe,
    TreePhotoComponent,
    PeopleListComponent,
    TreeOwnersComponent,
    PersonFilesFormComponent,
    TreeDrawComponent,
    LicensesComponent,
    PosterComponent,
    ImportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFireAuthModule,
    FormsModule,
    HttpClientModule,
    CommonModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
