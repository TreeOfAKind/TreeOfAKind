import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { AuthGuard } from './authentication/shared/auth.guard'
import { FormAction } from './helpers/form-action.enum';
import { PersonFilesFormComponent } from './people/person-files-form/person-files-form.component';
import { LicensesComponent } from './licenses/licenses.component';
import { PersonFormComponent } from './people/person-form/person-form.component';
import { TreeViewComponent } from './tree/tree-view/tree-view.component';
import { TreesListComponent } from './tree/trees-list/trees-list.component';
import { UserProfileFormComponent } from './user-profile/user-profile-form/user-profile-form.component';
import { PosterComponent } from './import-export/poster/poster.component';
import { ImportComponent } from './import-export/import/import.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'licenses', component: LicensesComponent },
  { path: '', canActivate: [AuthGuard], children: [
    { path: '', component: TreesListComponent },
    { path: 'user-profile', component: UserProfileFormComponent },
    { path: 'trees-list', component: TreesListComponent },
    { path: 'import', component: ImportComponent },
    { path: 'tree/:id', children: [
      { path: '', component: TreeViewComponent },
      { path: 'person-form/:id', component: PersonFormComponent, data: { formAction: FormAction.Edit } },
      { path: 'add-person', component: PersonFormComponent, data: { formAction: FormAction.Add } },
      { path: 'files/:id', component: PersonFilesFormComponent },
      { path: 'poster', component: PosterComponent },
    ] },
  ]},
  { path: '*', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
