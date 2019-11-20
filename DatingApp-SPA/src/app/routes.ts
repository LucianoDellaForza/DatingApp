import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component'
import { MessagesComponent } from './messages/messages.component'
import { ListsComponent } from './lists/lists.component'
import { AuthGuard } from './_guards/auth.guard'
import { MemberListComponent } from './members/member-list/member-list.component'
import { MemberDetailComponent } from './members/member-detail/member-detail.component'
import { MemberDetailResolver } from './_resolvers/member-detail.resolver'
import { MemberListResolver } from './_resolvers/member-list.resolver'
import { MemberEditComponent } from './members/member-edit/member-edit.component'
import { MemberEditResolver } from './_resolvers/member-edit.resolver'
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard'

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver} },
            { path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
            { path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent },
        ]
    },
    // { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] }, //moze canActivate: [AuthGuard] na sve pathove, ali ovo iznad je brzi nacin ako ima veliki broj pathova
    // { path: 'messages', component: MessagesComponent },
    // { path: 'lists', component: ListsComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }    //if no url matches above, it will go to this (home)
];
