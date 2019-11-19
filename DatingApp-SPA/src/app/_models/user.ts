import { Photo } from './photo';

export interface User {
    //for UserDto
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;

    //additional for UserDetailedDto - optionals with elvis operator
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
}
