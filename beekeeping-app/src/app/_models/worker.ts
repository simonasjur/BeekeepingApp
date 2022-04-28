export class Worker {
    farmId: number;
    userId: number;
    role: Role;
    firstName: string;
    lastName: string;
    permissions: string;
}

export enum Role {
    Owner, Assistant
}

export const Role2LabelMapping: Record<Role, string> = {
    [Role.Owner]: "Savininkas",
    [Role.Assistant]: "Asistentas"
};