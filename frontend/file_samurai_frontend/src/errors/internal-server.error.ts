import {HttpError} from "./http.error";

export class InternalServerError extends HttpError {
    constructor(data?: unknown | undefined, message = 'Internal Server Error') {
        super(message, 500, data);
    }
}