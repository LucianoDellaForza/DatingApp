import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/'; //'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService(); //jwtHelper instance from 3rd party JWTHelper library
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string) {
  this.photoUrl.next(photoUrl); //.next() method updates photoUrl
}

login(model: any) { //this method is called anonymously (no need for authorization), it returns token generated on server
  //token koji dodje u response-u, sacuvamo u localStorage valjda browsera
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((response: any) => {
      const user = response;
      if(user)
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user.user));  //set in storage alongside token, user 
        this.decodedToken = this.jwtHelper.decodeToken(user.token); //decoded token (now we have access to id, username(unique_name), nbf, exp, iat)
        this.currentUser = user.user;
        //console.log(this.decodedToken);
        this.changeMemberPhoto(this.currentUser.photoUrl);
    })
  )
}

register(model: any) {
  return this.http.post(this.baseUrl + 'register', model);
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);  //if token is expired or there is no token it returns !false
}

}
