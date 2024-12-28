import {HttpError} from "./http.error";

export class UnauthorizedError extends HttpError {
    constructor(message = 'Unauthorized access') {
        super(message, 401);
    }
}