export class Worker {
    id: number;
    farmId: number;
    userId: number;
    role: Role;
}

export enum Role {
    Owner, Assistant
}

export const Role2LabelMapping: Record<Role, string> = {
    [Role.Owner]: "Savininkas",
    [Role.Assistant]: "Asistentas"
};