import {HttpError} from "./http.error";

export class BadRequestError extends HttpError {
    constructor(data: unknown, message = 'Bad Request') {
        super(message, 400, data);
    }
}