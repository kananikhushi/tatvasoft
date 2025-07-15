import { Routes } from '@angular/router';
import { UserTypeGuard } from '../../guards/user-type.guard';
import { AuthGuard } from '../../guards/auth.guard';

import { DashboardComponent } from './dashboard/dashboard.component';
import { UserComponent } from './user/user.component';
import { MissionComponent } from './mission/mission.component';
import { MissionApplicationComponent } from './mission-application/mission-application.component';
import { MissionthemeComponent } from './mission-theme/missiontheme.component';
import { MissionskillComponent } from './mission-skill/missionskill.component';
import { AddMissionComponent } from './mission/add-mission/add-mission.component';
import { UpdateMissionComponent } from './mission/update-mission/update-mission.component';
import { AddEditMissionThemeComponent } from './mission-theme/add-edit-mission-theme/add-edit-mission-theme.component';
import { AddEditMissionSkillComponent } from './mission-skill/add-edit-mission-skill/add-edit-mission-skill.component';
import { AddUserComponent } from './user/add-user/add-user.component';
import { UpdateUserComponent } from './user/update-user/update-user.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {
    path: '',
    canActivateChild: [AuthGuard,UserTypeGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'user', component: UserComponent },
      { path: 'mission', component: MissionComponent },
      { path: 'missionApplication', component: MissionApplicationComponent },
      { path: 'missionTheme', component: MissionthemeComponent },
      { path: 'missionSkill', component: MissionskillComponent },
      { path: 'addMission', component: AddMissionComponent },
      { path: 'updateMission/:missionId', component: UpdateMissionComponent },
      { path: 'addMissionTheme', component: AddEditMissionThemeComponent },
      { path: 'updateMissionTheme/:id', component: AddEditMissionThemeComponent },
      { path: 'addMissionSkill', component: AddEditMissionSkillComponent },
      { path: 'updateMissionSkill/:id', component: AddEditMissionSkillComponent },
      { path: 'addUser', component: AddUserComponent },
      { path: 'updateUser/:userId', component: UpdateUserComponent },
      { path: 'updateProfile/:userId', component: UpdateUserComponent },
      { path: 'profile', component: ProfileComponent },
    ]
  }
];

export default routes;