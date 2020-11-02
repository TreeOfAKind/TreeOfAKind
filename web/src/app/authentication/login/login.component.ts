import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model = {
    email: '',
    password: ''
  }

  constructor(
    private authService: AuthService,
  ) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.authService.login(this.model.email, this.model.password);
  }

}
