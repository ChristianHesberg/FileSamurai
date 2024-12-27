import fs from "node:fs";
import {CreateFileUseCase} from "../use-cases/create-file.use-case";
import {CreateUserKeyPair} from "../use-cases/create-user-key-pair.use-case";
import {VIEWER_ROLE} from "../constants";
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {DecryptFileUseCase} from "../use-cases/decrypt-file.use-case";
import {ShareFileUseCase} from "../use-cases/share-file.use-case";

const fileService = new FileService();
const keyService = new KeyService();
const createFileUseCase = new CreateFileUseCase(
    fileService,
    keyService,
);
const decryptFileUseCase = new DecryptFileUseCase(
    fileService,
    keyService,
)
const shareFileUseCase = new ShareFileUseCase(
    fileService,
    keyService,
)

async function test(){
    fs.readFile('test.txt', async (err, data) => {
        if(err) console.log(err);
        if(data) await createFileUseCase.execute('e9171352-c0e3-4705-8f52-5afca618c8b2', 'f42eb234-8f11-4339-b6c5-7aca9a9091be', data, 'COOL TITLE');
    })
}

//createUserKeyPair('very_secret_password_that_you_cannot_guess', 'lame@email.com', 'c3ffaafd-3ee1-482b-9c39-99768c1d07db');
//test();
//decryptFile('e9171352-c0e3-4705-8f52-5afca618c8b2', '3a25fdfb-1b73-47de-8cea-b40b18b6b93a', 'cool@email.com-very_secret_password_that_you_cannot_guess');
//shareFile('e9171352-c0e3-4705-8f52-5afca618c8b2', 'c3ffaafd-3ee1-482b-9c39-99768c1d07db', '3a25fdfb-1b73-47de-8cea-b40b18b6b93a', 'cool@email.com-very_secret_password_that_you_cannot_guess', VIEWER_ROLE);
