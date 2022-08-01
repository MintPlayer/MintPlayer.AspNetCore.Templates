import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { Logout } from "./actions/logout";
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

}
