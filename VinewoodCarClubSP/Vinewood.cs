using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;

public class Vinewood : Script
{
    Vehicle[] veh = new Vehicle[10];
    Ped[] ped = new Ped[9];
    Vector3[] position = new Vector3[12];
    Vector3[] ped_position = new Vector3[9];
    float[] ped_heading = new float[9];

    String[] veh_name = new string[10];
    String[] ped_name = new string[9];
    String[] anim_name = new string[9];
    String[] anim_dict = new string[9];
    float[] veh_heading = new float[10];
    int[] veh_livery = new int[10];
    Blip club;
    int sprite = 838;
    int entered = 0;

    public Vinewood()
    {
        Tick += OnTick;

        Function.Call(Hash.ON_ENTER_MP);
        Function.Call(Hash.SET_INSTANCE_PRIORITY_MODE, 1);

        Function.Call(Hash.REQUEST_IPL, "m23_1_garage");
        LoadGarage();

        veh_name[0] = "seminole2";
        veh_heading[0] = 157.819f;
        veh_livery[0] = 9;

        veh_name[1] = "hakuchou2";
        veh_heading[1] = 157.819f;
        veh_livery[1] = 2;

        veh_name[2] = "stingertt";
        veh_heading[2] = 157.819f;
        veh_livery[2] = 8;

        veh_name[3] = "le7b";
        veh_heading[3] = 157.819f;
        veh_livery[3] = 1;

        veh_name[4] = "ratel";
        veh_heading[4] = 157.819f;
        veh_livery[4] = 6;


        veh_name[5] = "vigero2";
        veh_heading[5] = 52.89205f;
        veh_livery[5] = 7;

        veh_name[6] = "deveste";
        veh_heading[6] = 52.89205f;
        veh_livery[6] = 0;

        veh_name[7] = "cypher";
        veh_heading[7] = 52.89205f;
        veh_livery[7] = 14;

        veh_name[8] = "feltzer3";
        veh_heading[8] = 52.89205f;
        veh_livery[8] = 19;

        veh_name[9] = "monstrociti";
        veh_heading[9] = 318.1288f;
        veh_livery[9] = 3;

        ped_name[0] = "A_F_Y_CarClub_01";
        anim_dict[0] = "anim@amb@carmeet@checkout_car@female_b@base";
        anim_name[0] = "base";
        ped_position[0] = new Vector3(1206.819f, -3248.682f, -50.00f);
        ped_heading[0] = 43.86293f;


        ped_name[1] = "A_M_Y_CarClub_01";
        anim_dict[1] = "anim@amb@carmeet@checkout_car@male_a@base";
        anim_name[1] = "base";
        ped_position[1] = new Vector3(1201.903f, -3251.494f, -50.00f);
        ped_heading[1] = 349.7142f;

        ped_name[2] = "A_M_Y_CarClub_01";
        anim_dict[2] = "anim@amb@carmeet@checkout_car@male_b@idles";
        anim_name[2] = "idle_c";
        ped_position[2] = new Vector3(1200.211f, -3250.837f, -50.00f);
        ped_heading[2] = 328.1486f;

        ped_name[3] = "A_M_Y_CarClub_01";
        anim_dict[3] = "anim@amb@carmeet@checkout_car@male_a@idles";
        anim_name[3] = "idle_d";
        ped_position[3] = new Vector3(1189.059f, -3255.431f, -50.00f);
        ped_heading[3] = 60.43689f;

        ped_name[4] = "A_F_Y_CarClub_01";
        anim_dict[4] = "anim@amb@carmeet@checkout_car@male_c@base";
        anim_name[4] = "base";
        ped_position[4] = new Vector3(1188.694f, -3257.009f, -50.00f);
        ped_heading[4] = 59.67503f;

        ped_name[5] = "A_F_Y_CarClub_01";
        anim_dict[5] = "anim@amb@carmeet@checkout_car@male_a@base";
        anim_name[5] = "base";
        ped_position[5] = new Vector3(1195.416f, -3253.988f, -50.00f);
        ped_heading[5] = 191.1719f;

        ped_name[6] = "A_M_Y_CarClub_01";
        anim_dict[6] = "anim@amb@carmeet@checkout_car@male_b@idles";
        anim_name[6] = "idle_c";
        ped_position[6] = new Vector3(1200.378f, -3254.95f, -50.00f);
        ped_heading[6] = 161.8084f;

        ped_name[7] = "A_M_Y_CarClub_01";
        anim_dict[7] = "anim@amb@carmeet@checkout_car@female_b@base";
        anim_name[7] = "base";
        ped_position[7] = new Vector3(1206.134f, -3256.563f, -50.00f);
        ped_heading[7] = 135.4807f;

        ped_name[8] = "A_M_Y_CarClub_01";
        anim_dict[8] = "anim@amb@carmeet@checkout_car@male_d@base";
        anim_name[8] = "base";
        ped_position[8] = new Vector3(1203.26f, -3255.629f, -50.00f);
        ped_heading[8] = 226.9537f;


        position[0] = new Vector3(1210.22f, -3247.852f, -50.00f);
        position[1] = new Vector3(1206.22f, -3247.852f, -50.00f);
        position[2] = new Vector3(1202.22f, -3247.852f, -50.00f);
        position[3] = new Vector3(1198.22f, -3247.852f, -50.00f);
        position[4] = new Vector3(1194.22f, -3247.852f, -50.00f);
        position[5] = new Vector3(1205.278f, -3257.556f, -50.00f);
        position[6] = new Vector3(1201.278f, -3257.556f, -50.00f);
        position[7] = new Vector3(1197.278f, -3257.556f, -50.00f);
        position[8] = new Vector3(1193.278f, -3257.556f, -50.00f);

        position[9] = new Vector3(1218.465f, -3234.697f, 4.52875f); //вход маркер
        position[10] = new Vector3(1212.767f, -3252.277f, -49.99775f); //выход маркер

        position[11] = new Vector3(1182.436f, -3252.979f, -50.00f); //призовой транспорт (9)

        club = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[9].X, position[9].Y, position[9].Z);
        GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, club, 857);

    }

    private void LoadGarage()
    {
        int GarageID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1200.00f, -3250.00f, -50.00f);
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_backdrop_frames");
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_plus");
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_signs");
        Function.Call(Hash.REFRESH_INTERIOR, GarageID);
    }

    private void CreatePeds(int count)
    {
        int x = 0;
        for (int i = 0; i <= count; i++)
        {
            var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[i]));
            ped[i] = GTA.World.CreatePed(ped_model, ped_position[i], ped_heading[i]);
            ped[i].Task.PlayAnimation(anim_dict[i], anim_name[i], 8.0f, -1, GTA.AnimationFlags.Loop);
            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[i], true);
            x = i;
        }
        GTA.UI.Screen.ShowSubtitle("Peds " + x, 5000);
    }

    private void DeletePeds(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            ped[i].Delete();
        }
    }

    private void CreateVehicles(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            var veh_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, veh_name[i]));
            veh[i] = World.CreateVehicle(veh_model, position[i], veh_heading[i]);
            while (!veh_model.IsLoaded) Script.Wait(100);

            GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh[i]);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[i], 0);
            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, veh[i], 48, veh_livery[i], false);
            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, veh[i], true);
            veh_model.MarkAsNoLongerNeeded();
        }

        var prize_veh_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, veh_name[9]));

        veh[9] = World.CreateVehicle(prize_veh_model, position[11], veh_heading[9]);

        while (!prize_veh_model.IsLoaded) Script.Wait(100);
        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[9], 0);
        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, veh[9], 48, veh_livery[9], false);
        Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, veh[9], true);
        prize_veh_model.MarkAsNoLongerNeeded();
    }

    private void DeleteVehicles(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            if (Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, veh[i]))
            {
                Vector3 pos = new Vector3(1218.144f, -3226.999f, 5.88975f);
                Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, veh[i], false);
                veh[i].Position = pos;
                veh[i].Heading = 3.314108f;
                veh[i].MarkAsNoLongerNeeded();
            }
            else
            {
                veh[i].Delete();
                veh[i] = null;
            }
        }
        Function.Call(Hash.CLEAR_AREA_OF_VEHICLES, 1200.00f, -3250.00f, -50.00f, 300.0f, false, false, false, false, false, false);
    }

    private void OnTick(object sender, EventArgs e)
    {
        World.DrawMarker(MarkerType.VerticalCylinder, position[9], Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);
        World.DrawMarker(MarkerType.VerticalCylinder, position[10], Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);

        if (entered == 1)
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, true);
            Function.Call(GTA.Native.Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, 2725352035, true); //WEAPON_UNARMED
        }

        if (World.GetDistance(Game.Player.Character.Position, position[9]) < 1.5f)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
            while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
            Wait(1000);

            Vector3 pos = new Vector3(1210.181f, -3252.594f, -48.99775f);
            Game.Player.Character.Position = pos;
            Game.Player.Character.Heading = 100.7243f;
            CreateVehicles(8);
            CreatePeds(8);

            Wait(1000);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);

            entered = 1;
        }
        else
        {
            if (World.GetDistance(Game.Player.Character.Position, position[10]) < 1.5f)
            {
                Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
                Wait(1000);

                if(!Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Game.Player.Character))
                {
                    Vector3 pos = new Vector3(1218.144f, -3226.999f, 5.88975f);
                    Game.Player.Character.Position = pos;
                    Game.Player.Character.Heading = 3.314108f;
                }
                entered = 0;
                DeleteVehicles(9);
                DeletePeds(8);

                Wait(1000);

                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, false);
                Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                entered = 0;
            }
            else
            {
                if(Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, 71) && Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Game.Player.Character) && entered == 1)
                {
                    Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                    while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
                    Wait(1000);
                    entered = 0;
                    DeleteVehicles(9);
                    DeletePeds(8);

                    Wait(1000);
                    Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, false);
                }
            }
        }
    }
}