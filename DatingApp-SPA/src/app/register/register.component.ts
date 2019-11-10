import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;   //parent to child communication
  @Output() cancelRegister = new EventEmitter //child to parent communication - these properties emit event
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      // console.log('Registration successfull');
      this.alertify.success('Registration successfull')
    }, error => {
      // console.log(error)
      this.alertify.error(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);  //emiting boolean -> false
    //console.log('Canceled')
  }

}
