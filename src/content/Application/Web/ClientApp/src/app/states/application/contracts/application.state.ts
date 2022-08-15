import { User } from "../../../api/dtos/user";

export interface ApplicationState {
	user: User | null;
}
