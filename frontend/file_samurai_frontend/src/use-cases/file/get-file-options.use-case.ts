import {FileService} from "../../services/file.service";
import {FileOption} from "../../models/FileOption";

export class GetFileOptionsUseCase {
    constructor(private readonly fileService: FileService) {
    }

    async execute(userId:string): Promise<FileOption[]> {
        return await this.fileService.getFileOptions(userId)
    }
}