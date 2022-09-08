import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { patch } from '@ngxs/store/operators';
import { Logout } from "./actions/logout";
import { SetTwoFactor } from "./actions/set-two-factor";
import { SetBypassTwoFactor } from "./actions/set-two-factor-bypass";
import { SetUser } from "./actions/set-user";
import { ApplicationState } from "./contracts/application.state";
import { Metadata } from "./metadata";

@State<ApplicationState>({
	name: Metadata.namespace,
	defaults: {
		user: null
	}
})
@Injectable()
export class ApplicationManager {

	@Selector()
	static user(state: ApplicationState) {
		return state.user;
	}

	@Action(SetUser)
	setUser(ctx: StateContext<ApplicationState>, action: SetUser) {
		ctx.patchState({
			user: action.user
		});
	}

	@Action(Logout)
	logout(ctx: StateContext<ApplicationState>, action: Logout) {
		ctx.patchState({
			user: null
		});
	}

//#if (UseTwoFactorAuthentication)
	@Action(SetTwoFactor)
	setTwoFactor(ctx: StateContext<ApplicationState>, action: SetTwoFactor) {
		const applicationState = ctx.getState();
		if (applicationState.user) {
			ctx.setState({
				...applicationState,
				user: {
					...applicationState.user,
					isTwoFactorEnabled: action.isTwoFactorEnabled
				}
			});
		}
	}

	@Action(SetBypassTwoFactor)
	setBypassTwoFactor(ctx: StateContext<ApplicationState>, action: SetBypassTwoFactor) {
		const applicationState = ctx.getState();
		if (applicationState.user) {
			ctx.setState({
				...applicationState,
				user: {
					...applicationState.user,
					bypass2faForExternalLogin: action.bypassTwoFactorForExternallogin
				}
			});
		}
	}

//#endif
}
