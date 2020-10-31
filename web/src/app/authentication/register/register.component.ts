import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  model = {
    email: '',
    password: ''
  }

  constructor(
    private authService: AuthService,
    private  router:  Router
  ) { }

  ngOnInit(): void {
  }

  onSubmit(){
    this.authService.register(this.model.email, this.model.password);
  }

}
