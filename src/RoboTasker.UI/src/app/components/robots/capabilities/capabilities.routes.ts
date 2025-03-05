import {Routes} from '@angular/router';
import {CapabilitiesListComponent} from './capabilities-list/capabilities-list.component';
import {CapabilityAddOrUpdateComponent} from './capability-add-or-update/capability-add-or-update.component';

export const CAPABILITIES_ROUTES: Routes = [
  { path: '', component: CapabilitiesListComponent, data: { title: 'Capabilities' } },
  { path: 'add', component: CapabilityAddOrUpdateComponent, },
  { path: ':id', component: CapabilityAddOrUpdateComponent }
];
