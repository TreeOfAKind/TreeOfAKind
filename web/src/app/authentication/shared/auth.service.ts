import { Injectable, NgZone } from '@angular/core';
import { Router } from  "@angular/router";
import { AngularFireAuth } from  "@angular/fire/auth";
import { renderFlagCheckIfStmt } from '@angular/compiler/src/render3/view/template';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user = {
    email: null
  }

  constructor(
    private afAuth:  AngularFireAuth,
    private router:  Router,
    private zone: NgZone
  ) { }

  async register(email: string, password: string) {
    await this.afAuth.createUserWithEmailAndPassword(email, password)
      .then(result => {
        this.user.email = result.user.email;
        this.zone.run(() => this.router.navigate(['/login']));
      })
      .catch(error => {
        window.alert(error.message);
      });
  }
}
