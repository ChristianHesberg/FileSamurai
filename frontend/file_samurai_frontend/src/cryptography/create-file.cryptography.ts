import {generateKey} from "./utils.cryptography";
import {encryptAes256Gcm} from "./aes-256-gcm.cryptography"
import {AddFileDto} from "../models/addFileDto";
import {encryptWithPublicKey} from "./rsa.cryptography";
import {AddUserFileAccessDto} from "../models/addUserFileAccessDto";
import {EDITOR_ROLE} from "../../constants";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import * as fs from "node:fs";
import axiosInstance from "../api/axios-instance";

export async function createFile(userId: string, groupId: string, file: Buffer, title: string){
    const key = generateKey(32);
    const encryptedFileResponse = encryptFile(file, key, title, groupId);
    const fileResponse: AddFileResponseDto = await postFile(encryptedFileResponse);

    const userPublicKey: string = await getUserPublicKey(userId);
    console.log(userPublicKey);
    const encryptedFAK = encryptWithPublicKey(key, userPublicKey);

    const addUserFileAccessDto: AddUserFileAccessDto = generateUserFileAccessDto(encryptedFAK, userId, fileResponse.id);
    await postUserFileAccess(addUserFileAccessDto);
}

const encryptFile = (file: Buffer, key: Buffer, title: string, groupId: string): AddFileDto => {
    const encryptedFile = encryptAes256Gcm(file, key);
    return {
        fileContents: encryptedFile.cipherText,
        nonce: encryptedFile.nonce,
        tag: encryptedFile.tag,
        title: title,
        groupId: groupId
    }
}

const generateUserFileAccessDto = (encryptedFAK: Buffer, userId: string, fileId: string): AddUserFileAccessDto => {
    return {
        encryptedFileKey: encryptedFAK.toString('base64'),
        role: EDITOR_ROLE,
        userId: userId,
        fileId: fileId
    }
}

async function postFile(dto: AddFileDto): Promise<AddFileResponseDto> {
    const response = await axiosInstance.post<AddFileResponseDto>(`file`, dto);
    return response.data;
}

async function getUserPublicKey(userId: string): Promise<string> {
    const response = await axiosInstance.get<string>(`keypair/public/${userId}`, {
        headers: {
            'Accept': 'text/plain',
        },
    });
    return response.data;
}

async function postUserFileAccess(dto: AddUserFileAccessDto): Promise<void> {
    await axiosInstance.post<AddUserFileAccessDto>('file/access', dto);
}

function test(){
    fs.readFile('test.txt', (err, data) => {
        if(err) console.log(err);
        if(data) createFile('e9171352-c0e3-4705-8f52-5afca618c8b2', 'f42eb234-8f11-4339-b6c5-7aca9a9091be', data, 'COOL TITLE');
    })
}

test();



