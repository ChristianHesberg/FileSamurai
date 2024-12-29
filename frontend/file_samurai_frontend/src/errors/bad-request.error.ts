import {HttpError} from "./http.error";

export class BadRequestError extends HttpError {
    constructor(data?: unknown | undefined, message = 'Bad Request') {
        super(message, 400, data);
    }
}