import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { } //public authService zbog nav.html componente ({{authService.decodedToken.unique_name}} u njoj)

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      //console.log('Logged in successfully');
      this.alertify.success("Logged in successfully");
    }, error => {
      //console.log(error);
      this.alertify.error(error);
    }, () => {  //ovo je complete (3. argument (opcioni) .subscribe metode, u koji se stavlja sta da se radi kad se izvrsi uspesno metoda - ovo je moglo i da ide gore odmah posle this.alertify.success("Logged in successfully");)
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    // const token = localStorage.getItem('token');
    // return !!token; //vraca true ako ima tokena, false ako nema

    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    //console.log('logged out');
    this.alertify.message('Logged out ');
    this.router.navigate(['/home']);
  }

}
