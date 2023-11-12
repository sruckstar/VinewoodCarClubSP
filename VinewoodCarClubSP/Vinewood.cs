using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GTA;
using GTA.Native;
using GTA.Math;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Elements;
using LemonUI.Extensions;

public class Vinewood : Script
{
    Vehicle[] veh = new Vehicle[10];
    Ped[] ped = new Ped[9];

    Vector3[] position = new Vector3[13];
    Vector3[] ped_position = new Vector3[9];
    float[] ped_heading = new float[9];
    NativeMenu menu;
    NativeMenu[] EditMenu = new NativeMenu[10];
    NativeSubmenuItem[] car_model = new NativeSubmenuItem[10];
    NativeListItem<string> SubvehName;
    NativeListItem<int> SubvehLivery;
    NativeListItem<string> itemCLT;
    NativeItem SubvehDelete;

    String[] veh_name = new string[10];
    String[] ped_name = new string[9];
    String[] anim_name = new string[9];
    String[] anim_dict = new string[9];
    float[] veh_heading = new float[10];
    int[] veh_livery = new int[10];
    Blip[] debug_peds = new Blip[9];
    Blip club;
    Blip prize;
    Blip GarageKeys;
    int sprite = 838;
    int entered = 0;
    int PlayerLoad = 0;
    int mainMenuCreated = 0;
    int editMenuCreated = 0;
    int stateMainMenu = -1;
    int confirm = -1;
    int BadgeLocked;
    int update_available = 0;
    string downloaded_response;
    Camera cumera;
    string temp_colortype = null;

    static string path = @".\scripts\VineWoodClubCars.ini";
    static string github_updates = "https://raw.githubusercontent.com/sruckstar/VinewoodCarClubSP/main/system_updates/selection-information.md";
    static string github_models = "https://raw.githubusercontent.com/sruckstar/VinewoodCarClubSP/main/system_updates/models.md";
    ScriptSettings config = ScriptSettings.Load(path);
    private static readonly ObjectPool pool = new ObjectPool();

    public Vinewood()
    {
        Tick += OnTick;
        Aborted += OnAborted;

        Function.Call(Hash.ON_ENTER_MP);
        Function.Call(Hash.SET_INSTANCE_PRIORITY_MODE, 1);

        menu = new NativeMenu("Vinewood Car Club");
        CreateMainMenu();

        Function.Call(Hash.REQUEST_IPL, "m23_1_garage");
        LoadGarage();

        pool.Add(menu);

        veh_heading[0] = 318.1288f;
        veh_heading[1] = -144.9848f;
        veh_heading[2] = -161.2509f;
        veh_heading[3] = -176.253f;
        veh_heading[4] = 178.2071f;
        veh_heading[5] = 164.4194f;
        veh_heading[6] = -4.000244f;
        veh_heading[7] = -4.035083f;
        veh_heading[8] = -2.826211f;
        veh_heading[9] = 1.158939f;

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

        position[0] = new Vector3(1182.436f, -3252.979f, -50.00f); //prize veh
        position[1] = new Vector3(1192.5f, -3249.51f, -50.70123f);
        position[2] = new Vector3(1195.515f, -3248.417f, -50.70127f);
        position[3] = new Vector3(1198.6f, -3247.779f, -50.70224f);
        position[4] = new Vector3(1201.851f, -3247.727f, -50.70224f);
        position[5] = new Vector3(1205.24f, -3247.87f, -50.70234f);
        position[6] = new Vector3(1204f, -3257.981f, -50.6998f);
        position[7] = new Vector3(1200.251f, -3257.721f, -50.70198f);
        position[8] = new Vector3(1196.536f, -3257.526f, -50.70204f);
        position[9] = new Vector3(1192.206f, -3257.731f, -50.70211f);
        //enter exit markers
        position[10] = new Vector3(1218.465f, -3234.697f, 4.52875f); 
        position[11] = new Vector3(1212.767f, -3252.277f, -49.99775f);
        //edit garage
        position[12] = new Vector3(1211.326f, -3258.668f, -49.99775f);

        
        if (club == null)
        {
            club = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[10].X, position[10].Y, position[10].Z);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, club, 857);
        }
    }

    private void CreateMainMenu()
    {
        string car_ini;
        string label_text;
        string VehName;
        VehicleHash model_hash;

        for (int i = 0; i <= 9; i++)
        {
            car_ini = $"CAR_{i}";
            string tempVeh = config.GetValue<string>(car_ini, "model", "Empty");
            int tempLivery = config.GetValue<int>(car_ini, "livery", -1);
            int tempColor_1 = config.GetValue<int>(car_ini, "COLOR_1", -1);
            int tempColor_2 = config.GetValue<int>(car_ini, "COLOR_2", -1);

            if (tempVeh != "Empty")
            {
                model_hash = Function.Call<VehicleHash>(Hash.GET_HASH_KEY, tempVeh);
                label_text = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, model_hash);
                VehName = Game.GetLocalizedString(label_text);
            }
            else
            {
                VehName = "Empty";
            }

            EditMenu[i] = new NativeMenu("Edit Vehicle");
            NativeListItem<string> itemA = new NativeListItem<string>("Model:", "", tempVeh);
            NativeListItem<int> itemB = new NativeListItem<int>("Livery ID:", "", tempLivery);
            itemCLT = new NativeListItem<string>("Color Type:", "", "Normal", "Metallic", "Pearl", "Matte", "Metal", "Chrome", "Chameleon");
            NativeListItem<int> itemCLR1 = new NativeListItem<int>("Primary Color:", "", tempColor_1);
            NativeListItem<int> itemCLR2 = new NativeListItem<int>("Secondary Color:", "", tempColor_2);
            NativeItem itemC = new NativeItem("Delete Vehicle"); 

            BadgeSet shop_lock = new BadgeSet
            {
                NormalDictionary = "commonmenu",
                NormalTexture = "shop_lock",
                HoveredDictionary = "commonmenu",
                HoveredTexture = "shop_lock"
            };

            if (tempVeh == "Empty")
            {
                itemC.RightBadgeSet = shop_lock;
            }

            EditMenu[i].Add(itemA);
            itemA.Activated += (sender, args) =>
            {
                Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, false, "FMMC_KEY_TIP", "", "", "", "", "", 16);

                while(true)
                {
                    Script.Wait(0);
                    string result = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                    int state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                    if (result != null && state == 1)
                    {
                        model_hash = Function.Call<VehicleHash>(Hash.GET_HASH_KEY, result);
                        if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, model_hash))
                        {
                            car_ini = $"CAR_{stateMainMenu}";
                            config.SetValue(car_ini, "model", result);
                            config.Save();
                            tempLivery = config.GetValue<int>(car_ini, "livery", -1);
                            var model_list = new List<string>() { result };
                            itemA.Items = model_list;
                            VehName = Game.GetLocalizedString(result);
                            car_model[stateMainMenu].Title = VehName;

                            if(veh[stateMainMenu] != null)
                            {
                                veh[stateMainMenu].Delete();
                            }

                            var veh_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, result));
                            veh[stateMainMenu] = World.CreateVehicle(veh_model, position[stateMainMenu], veh_heading[stateMainMenu]);
                            while (!veh_model.IsLoaded) Script.Wait(100);

                            GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh[stateMainMenu]);
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, veh[stateMainMenu], 48, tempLivery, false);
                            Wait(100);
                            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, veh[stateMainMenu], true);
                            veh_model.MarkAsNoLongerNeeded();
                            itemC.RightBadgeSet = null;
                            CreatePedsAdvanced(stateMainMenu);
                            break;
                        }
                        else
                        {
                            GTA.UI.Screen.ShowSubtitle("Incorrect model name!");
                            break;
                        }
                    }
                    else
                    {
                        state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                        if (state == -1 || state == 2)
                        {
                            //Exit from the on screen keyboard
                            break;
                        }
                    }
                }
            };

           EditMenu[i].Add(itemB);
           itemB.Activated += (sender, args) =>
           {
               Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, false, "FMMC_KEY_TIP", "", "", "", "", "", 16);

               while (true)
               {
                   Script.Wait(0);
                   string result = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                   int state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                   if (result != null && state == 1)
                   {
                       bool isNumeric = int.TryParse(result, out int livertID);
                       if (isNumeric)
                       {
                           Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                           Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, veh[stateMainMenu], 48, livertID, false);
                           car_ini = $"CAR_{stateMainMenu}";
                           config.SetValue(car_ini, "livery", livertID);
                           config.Save();
                           var model_list = new List<int>() { livertID };
                           itemB.Items = model_list;
                           break;
                       }
                       else
                       {
                           GTA.UI.Screen.ShowSubtitle("Incorrect value!");
                           break;
                       }
                   }
                   else
                   {
                       state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                       if (state == -1 || state == 2)
                       {
                           //Exit from the on screen keyboard
                           break;
                       }
                   }
               }
           };

            EditMenu[i].Add(itemCLR1);
            itemCLR1.Activated += (sender, args) =>
            {
                Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, false, "FMMC_KEY_TIP", "", "", "", "", "", 16);

                while (true)
                {
                    Script.Wait(0);
                    string result = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                    int state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                    if (result != null && state == 1)
                    {
                        bool isNumeric = int.TryParse(result, out int COLOR_1);
                        if (isNumeric)
                        {
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_1, veh[stateMainMenu], itemCLT.SelectedIndex, COLOR_1, 0);
                            car_ini = $"CAR_{stateMainMenu}";
                            config.SetValue(car_ini, "COLOR_1", COLOR_1);
                            config.Save();
                            var model_list = new List<int>() { COLOR_1 };
                            itemCLR1.Items = model_list;
                            break;
                        }
                        else
                        {
                            GTA.UI.Screen.ShowSubtitle("Incorrect value!");
                            break;
                        }
                    }
                    else
                    {
                        state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                        if (state == -1 || state == 2)
                        {
                            //Exit from the on screen keyboard
                            break;
                        }
                    }
                }
            };

            EditMenu[i].Add(itemCLR2);
            itemCLR2.Activated += (sender, args) =>
            {
                Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, false, "FMMC_KEY_TIP", "", "", "", "", "", 16);

                while (true)
                {
                    Script.Wait(0);
                    string result = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                    int state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                    if (result != null && state == 1)
                    {
                        bool isNumeric = int.TryParse(result, out int COLOR_2);
                        if (isNumeric)
                        {
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                            Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_2, veh[stateMainMenu], itemCLT.SelectedIndex, COLOR_2, 0);
                            car_ini = $"CAR_{stateMainMenu}";
                            config.SetValue(car_ini, "COLOR_2", COLOR_2);
                            config.Save();
                            var model_list = new List<int>() { COLOR_2 };
                            itemCLR2.Items = model_list;
                            break;
                        }
                        else
                        {
                            GTA.UI.Screen.ShowSubtitle("Incorrect value!");
                            break;
                        }
                    }
                    else
                    {
                        state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                        if (state == -1 || state == 2)
                        {
                            //Exit from the on screen keyboard
                            break;
                        }
                    }
                }
            };

            EditMenu[i].Add(itemC);
           itemC.Activated += (sender, args) =>
           {
               if (confirm != 1)
               {
                   if (veh[stateMainMenu] != null)
                   {
                       itemC.Description = "Are you sure?";
                       EditMenu[stateMainMenu].Visible = true;
                       confirm = 1;
                       editMenuCreated = 1;
                   }
                   else
                   {
                       itemC.Description = "The vehicle has already been removed";
                       EditMenu[stateMainMenu].Visible = true;
                       confirm = 1;
                       editMenuCreated = 1;
                   }
               }
               else
               {
                   if (veh[stateMainMenu] != null && confirm == 1)
                   {
                       veh[stateMainMenu].Delete();
                       veh[stateMainMenu] = null;
                       DeletePedsAdvanced(stateMainMenu);
                       car_ini = $"CAR_{stateMainMenu}";
                       config.SetValue(car_ini, "model", "Empty");
                       config.SetValue(car_ini, "livery", -1);
                       config.Save();
                       confirm = -1;
                       itemC.Description = "";
                       shop_lock = new BadgeSet
                       {
                           NormalDictionary = "commonmenu",
                           NormalTexture = "shop_lock",
                           HoveredDictionary = "commonmenu",
                           HoveredTexture = "shop_lock"
                       };
                       itemC.RightBadgeSet = shop_lock;
                       var model_list = new List<string>() { "Empty" };
                       itemA.Items = model_list;
                       EditMenu[stateMainMenu].Visible = false;
                       editMenuCreated = 0;
                       List<NativeItem> items = menu.Items;
                       items[stateMainMenu].Title = "Empty";
                       menu.Visible = true;
                   }
               }
           };


            pool.Add(EditMenu[i]);
            car_model[i] = menu.AddSubMenu(EditMenu[i]);
            car_model[i].Title = VehName;
            car_model[i].Activated += (sender, args) =>
            {
                int x = menu.SelectedIndex;
                EditMenu[x].Visible = true;
                editMenuCreated = 1;
            };

            if (i == 0)
            {
                BadgeSet star = new BadgeSet
                {
                    NormalDictionary = "commonmenu",
                    NormalTexture = "shop_new_star",
                    HoveredDictionary = "commonmenu",
                    HoveredTexture = "shop_new_star"
                };
                car_model[i].LeftBadgeSet = star;
            }
        }
        mainMenuCreated = 1;
    }

    private void CreateCameraMode()
    {
        Hash camHash = Function.Call<Hash>(Hash.GET_HASH_KEY, "DEFAULT_SCRIPTED_CAMERA");
        cumera = GTA.Native.Function.Call<Camera>(GTA.Native.Hash.CREATE_CAMERA_WITH_PARAMS, camHash, position[11].X, position[11].Y, position[11].Z + 5.0f, 0.0f, 0.0f, 0.0f, 70.0f, true, 2);
        Function.Call(Hash.SET_CAM_ACTIVE, cumera, true);
        Function.Call(Hash.POINT_CAM_AT_COORD, cumera, position[0].X, position[0].Y, position[0].Z);
        Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 0, true, false, 0);
    }

    private void LoadGarage()
    {
        int GarageID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1200.00f, -3250.00f, -50.00f);
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_backdrop_frames");
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_plus");
        Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, GarageID, "entity_set_signs");
        Function.Call(Hash.REFRESH_INTERIOR, GarageID);
    }

    private void CreatePedsAdvanced(int typegroup)
    {
        if (typegroup == 0)
        {
            if (ped[3] == null)
            {
                var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[3]));
                ped[3] = GTA.World.CreatePed(ped_model, ped_position[3], ped_heading[3]);
                ped[3].Task.PlayAnimation(anim_dict[3], anim_name[3], 8.0f, -1, GTA.AnimationFlags.Loop);
                if (ped[3] != null)
                {
                    Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[3], true);
                }
            }

            if (ped[4] == null)
            {
                var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[4]));
                ped[4] = GTA.World.CreatePed(ped_model, ped_position[4], ped_heading[4]);
                ped[4].Task.PlayAnimation(anim_dict[4], anim_name[4], 8.0f, -1, GTA.AnimationFlags.Loop);
                if (ped[4] != null)
                {
                    Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[4], true);
                }
            }
        }
        else
        {
            if (typegroup == 2)
            {
                if (ped[0] == null)
                {
                    var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[0]));
                    ped[0] = GTA.World.CreatePed(ped_model, ped_position[0], ped_heading[0]);
                    ped[0].Task.PlayAnimation(anim_dict[0], anim_name[0], 8.0f, -1, GTA.AnimationFlags.Loop);
                    if (ped[0] != null)
                    {
                        Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[0], true);
                    }
                }
            }
            else
            {
                if (typegroup == 3)
                {
                    if (ped[1] == null)
                    {
                        var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[1]));
                        ped[1] = GTA.World.CreatePed(ped_model, ped_position[1], ped_heading[1]);
                        ped[1].Task.PlayAnimation(anim_dict[1], anim_name[1], 8.0f, -1, GTA.AnimationFlags.Loop);
                        if (ped[1] != null)
                        {
                            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[1], true);
                        }
                    }

                    if (ped[2] == null)
                    {
                        var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[2]));
                        ped[2] = GTA.World.CreatePed(ped_model, ped_position[2], ped_heading[2]);
                        ped[2].Task.PlayAnimation(anim_dict[2], anim_name[2], 8.0f, -1, GTA.AnimationFlags.Loop);
                        if (ped[2] != null)
                        {
                            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[2], true);
                        }
                    }
                }
                else
                {
                    if (typegroup == 6)
                    {
                        if (ped[7] == null)
                        {
                            var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[7]));
                            ped[7] = GTA.World.CreatePed(ped_model, ped_position[7], ped_heading[7]);
                            ped[7].Task.PlayAnimation(anim_dict[7], anim_name[7], 8.0f, -1, GTA.AnimationFlags.Loop);
                            if (ped[7] != null)
                            {
                                Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[7], true);
                            }
                        }

                        if (ped[8] == null)
                        {
                            var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[8]));
                            ped[8] = GTA.World.CreatePed(ped_model, ped_position[8], ped_heading[8]);
                            ped[8].Task.PlayAnimation(anim_dict[8], anim_name[8], 8.0f, -1, GTA.AnimationFlags.Loop);
                            if (ped[8] != null)
                            {
                                Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[8], true);
                            }
                        }
                    }
                    else
                    {
                        if (typegroup == 7)
                        {
                            if (ped[6] == null)
                            {
                                var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[6]));
                                ped[6] = GTA.World.CreatePed(ped_model, ped_position[6], ped_heading[6]);
                                ped[6].Task.PlayAnimation(anim_dict[6], anim_name[6], 8.0f, -1, GTA.AnimationFlags.Loop);
                                if (ped[6] != null)
                                {
                                    Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[6], true);
                                }
                            }
                        }
                        else
                        {
                            if (typegroup == 8)
                            {
                                if (ped[5] == null)
                                {
                                    var ped_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, ped_name[5]));
                                    ped[5] = GTA.World.CreatePed(ped_model, ped_position[5], ped_heading[5]);
                                    ped[5].Task.PlayAnimation(anim_dict[5], anim_name[5], 8.0f, -1, GTA.AnimationFlags.Loop);
                                    if (ped[5] != null)
                                    {
                                        Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, ped[5], true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void DeletePeds(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            if (ped[i] != null)
            {
                ped[i].Delete();
                ped[i] = null;
            }
        }
    }
    
    private void DeletePedsAdvanced(int typegroup)
    {
        if(typegroup == 0)
        {
            if (ped[3] != null)
            {
                ped[3].Delete();
                ped[3] = null;
            }

            if (ped[4] != null)
            {
                ped[4].Delete();
                ped[4] = null;
            }
        }
        else
        {
            if(typegroup == 2)
            {
                if (ped[0] != null)
                {
                    ped[0].Delete();
                    ped[0] = null;
                }
            }
            else
            {
                if (typegroup == 3)
                {
                    if (ped[1] != null)
                    {
                        ped[1].Delete();
                        ped[1] = null;
                    }

                    if (ped[2] != null)
                    {
                        ped[2].Delete();
                        ped[2] = null;
                    }
                }
                else
                {
                    if (typegroup == 6)
                    {
                        if (ped[7] != null)
                        {
                            ped[7].Delete();
                            ped[7] = null;
                        }

                        if (ped[8] != null)
                        {
                            ped[8].Delete();
                            ped[8] = null;
                        }
                    }
                    else
                    {
                        if (typegroup == 7)
                        {
                            if (ped[6] != null)
                            {
                                ped[6].Delete();
                                ped[6] = null;
                            }
                        }
                        else
                        {
                            if (typegroup == 8)
                            {
                                if (ped[5] != null)
                                {
                                    ped[5].Delete();
                                    ped[5] = null;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void CreateVehicles(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            string car_ini = $"CAR_{i}";
            string tempVeh = config.GetValue<string>(car_ini, "model", "Empty");
            int tempLivery = config.GetValue<int>(car_ini, "livery", -1);
            int tempColor_type = config.GetValue<int>(car_ini, "COLOR_TYPE", 0);
            int tempColor_1 = config.GetValue<int>(car_ini, "COLOR_1", -1);
            int tempColor_2 = config.GetValue<int>(car_ini, "COLOR_2", -1);

            if (tempVeh != "Empty")
            {
                var veh_model = new Model(Function.Call<VehicleHash>(Hash.GET_HASH_KEY, tempVeh));
                veh[i] = World.CreateVehicle(veh_model, position[i], veh_heading[i]);
                int time = Game.GameTime;
                int error = 0;

                while (veh[i] == null)
                {
                    Script.Wait(0);
                    if (Game.GameTime > time + 10000)
                    {
                        error = 1;
                        break;
                    }
                }

                if (error == 0)
                {
                    config.SetValue(car_ini, "TEMP_HANDLE", veh[i]);
                    config.Save();
                    GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh[i]);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[i], 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, veh[i], 48, tempLivery, false);
                    if (tempColor_1 != -1)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_1, veh[i], tempColor_type, tempColor_1, 0);
                    }
                    if (tempColor_2 != -1)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_2, veh[i], tempColor_type, tempColor_2, 0);
                    }
                    Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, veh[i], true);
                    veh_model.MarkAsNoLongerNeeded();
                    CreatePedsAdvanced(i);
                }
                else
                {
                    Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                    GTA.UI.Screen.ShowSubtitle("Car loading is taking longer than usual...", 10000);
                }
            }
            else
            {
                veh[i] = null;
            }
            
        }
    }

    private void DeleteVehicles(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            if(veh[i] != null)
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
        }
    }

    private void OnAborted(object sender, EventArgs e)
    {
        //Delete markers
        if (club != null && club.Exists())
        {
            club.Delete();
        }
        if (GarageKeys != null && GarageKeys.Exists())
        {
            GarageKeys.Delete();
        }
        if (prize != null && prize.Exists())
        {
            prize.Delete();
        }

        //Delete cars
        foreach (Vehicle car in veh)
        {
            if (car != null && car.Exists() && !Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, car))
            {
                car.Delete();
            }
        }

        //Delete peds
        foreach (Ped chel in ped)
        {
            if (chel != null && chel.Exists())
            {
                chel.Delete();
            }
        }

        //Cameras delete
        if (cumera != null && cumera.Exists())
        {
            Function.Call(Hash.DESTROY_CAM, cumera);
            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0);
            Function.Call(Hash.FREEZE_ENTITY_POSITION, Game.Player.Character, false);
            cumera = null;
        }

        //Fast exit from garage
        if (entered == 1 || PlayerLoad == 1)
        {
            Vector3 pos = new Vector3(1218.144f, -3226.999f, 5.88975f);
            Game.Player.Character.Position = pos;
            Game.Player.Character.Heading = 3.314108f;
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, false);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, false);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, false);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 45, false);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, false);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
        }

        //Print error
        if (PlayerLoad == 1)
        {
            GTA.UI.Screen.ShowHelpText("Something went wrong. Please try reloading the script.");
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        pool.Process();

        if (menu.Visible)
        {
            stateMainMenu = menu.SelectedIndex;
            Vector3 mark = new Vector3(position[stateMainMenu].X, position[stateMainMenu].Y, position[stateMainMenu].Z + 3.0f);
            World.DrawMarker(MarkerType.UpsideDownCone, mark, Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);
        }
        if (!menu.Visible && mainMenuCreated == 1)
        {
            mainMenuCreated = 0;
        }

        if (editMenuCreated == 1 && EditMenu[stateMainMenu].Visible)
        {
            int x = EditMenu[stateMainMenu].SelectedIndex;
            if (x == 2)
            {
                temp_colortype = itemCLT.SelectedItem;
            }
            else
            {
                if (temp_colortype != null)
                {
                    int color_type_int = 0;
                    switch (temp_colortype)
                    {
                        case "Normal":
                            color_type_int = 0;
                            break;

                        case "Metallic":
                            color_type_int = 1;
                            break;

                        case "Pearl":
                            color_type_int = 2;
                            break;

                        case "Matte":
                            color_type_int = 3;
                            break;

                        case "Metal":
                            color_type_int = 4;
                            break;

                        case "Chrome":
                            color_type_int = 5;
                            break;

                        case "Chameleon":
                            color_type_int = 6;
                            break;

                    }

                    string car_ini = $"CAR_{stateMainMenu}";
                    config.SetValue(car_ini, "COLOR_TYPE", color_type_int);
                    config.Save();
                    int tempColor_1 = config.GetValue<int>(car_ini, "COLOR_1", -1);
                    int tempColor_2 = config.GetValue<int>(car_ini, "COLOR_2", -1);
                    if (tempColor_1 != -1)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_1, veh[stateMainMenu], color_type_int, tempColor_1, 0);
                    }
                    if (tempColor_2 != -1)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, veh[stateMainMenu], 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_COLOR_2, veh[stateMainMenu], color_type_int, tempColor_2, 0);
                    }
                    temp_colortype = null;
                }
            }
        }

        if (confirm == 1 && EditMenu[stateMainMenu].Visible)
        {
            int x = EditMenu[stateMainMenu].SelectedIndex;
            List<NativeItem> items = EditMenu[stateMainMenu].Items;

            if (x != 4)
            {
                confirm = -1;
                items[4].Description = "";
            }
        }

        if (!menu.Visible && cumera != null)
        {
            int state = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
            if (Function.Call<bool>(Hash.DOES_CAM_EXIST, cumera) && !EditMenu[stateMainMenu].Visible && state != 0)
            {
                Function.Call(Hash.DESTROY_CAM, cumera);
                Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0);
                Function.Call(Hash.FREEZE_ENTITY_POSITION, Game.Player.Character, false);
                cumera = null;
            }
        }

        World.DrawMarker(MarkerType.VerticalCylinder, position[10], Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);
        World.DrawMarker(MarkerType.VerticalCylinder, position[11], Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);
        World.DrawMarker(MarkerType.VerticalCylinder, position[12], Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.LightBlue);

        if (entered == 1)
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 45, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, true);
            Function.Call(GTA.Native.Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, 2725352035, true); //WEAPON_UNARMED
        }

        if (World.GetDistance(Game.Player.Character.Position, position[10]) < 1.5f)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
            while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
            Wait(1000);
            PlayerLoad = 1;
            Vector3 pos = new Vector3(1210.181f, -3252.594f, -48.99775f);
            Game.Player.Character.Position = pos;
            Game.Player.Character.Heading = 100.7243f;

            //Check GTAO cars selection
            string response = null;
            int download = config.GetValue<int>("SETTINGS", "CHECK_UPDATES", 1);
            if (download == 1)
            {
                WebClient ws = new WebClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                response = ws.DownloadString(github_updates);
                while (response == null) Script.Wait(0);
                downloaded_response = response;
            }
            CreateVehicles(9);

            if(veh[0] != null)
            {
                prize = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[0].X, position[0].Y, position[0].Z);
                GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, prize, 781);
            }
            GarageKeys = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[12].X, position[12].Y, position[12].Z);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, GarageKeys, 811);
            club.Delete();

            Wait(1000);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);

            string current_selection = config.GetValue<string>("SETTINGS", "current_selection", "Empty");
            if (download == 1 && current_selection != response)
            {
                GTA.UI.Screen.ShowHelpText("A new update (" + response + ")" + " is available. Press ~INPUT_FRONTEND_Y~ to update the garage.");
                update_available = 1;
            }
            PlayerLoad = 0;
            entered = 1;
        }
        else
        {
            if (World.GetDistance(Game.Player.Character.Position, position[11]) < 1.5f)
            {
                update_available = 0;
                Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
                Wait(1000);

                if(!Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Game.Player.Character))
                {
                    Vector3 pos = new Vector3(1218.144f, -3226.999f, 5.88975f);
                    Game.Player.Character.Position = pos;
                    Game.Player.Character.Heading = 3.314108f;
                }
                DeleteVehicles(9);
                DeletePeds(8);
                club = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[10].X, position[10].Y, position[10].Z);
                GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, club, 857);

                if (prize != null)
                {
                    prize.Delete();
                }

                if (GarageKeys != null)
                {
                    GarageKeys.Delete();
                }

                Wait(1000);

                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 45, false);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, false);
                Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                entered = 0;
            }
            else
            {
                if(Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, 71) && Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Game.Player.Character) && entered == 1)
                {
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 23, true);
                    Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                    while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);

                    club = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[10].X, position[10].Y, position[10].Z);
                    GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, club, 857);
                    if (prize != null)
                    {
                        prize.Delete();
                    }
                    if (GarageKeys != null)
                    {
                        GarageKeys.Delete();
                    }

                    foreach (Vehicle car in veh)
                    {
                        if (car != null && car.Exists() && Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, car))
                        {
                            car.MarkAsNoLongerNeeded();
                            break;
                        }
                    }

                    Wait(1000);
                    entered = 0;
                    DeleteVehicles(9);
                    DeletePeds(8);

                    Wait(1000);
                    Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 22, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 23, true);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 24, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 45, false);
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 199, false);
                }
                else
                {
                    if(World.GetDistance(Game.Player.Character.Position, position[12]) < 1.5f && Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, 38) && cumera == null)
                    {
                        Function.Call(Hash.DESTROY_MOBILE_PHONE);
                        Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, Game.Player.Character);
                        Function.Call(Hash.FREEZE_ENTITY_POSITION, Game.Player.Character, true);
                        CreateCameraMode();
                        menu.Visible = true;
                    }
                    else
                    {
                        if (World.GetDistance(Game.Player.Character.Position, position[12]) < 1.5f && cumera == null)
                        {
                            GTA.UI.Screen.ShowHelpTextThisFrame("Press ~INPUT_PICKUP~ to open the garage control menu.");
                        }
                        else
                        {
                            if (update_available == 1 && Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, 204) && !menu.Visible)
                            {
                                Function.Call(Hash.DO_SCREEN_FADE_OUT, 1000);
                                while (Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT) == false) Script.Wait(100);
                                Wait(1000);
                                DeleteVehicles(9);
                                DeletePeds(8);
                                if (prize != null)
                                {
                                    prize.Delete();
                                }

                                if (GarageKeys != null)
                                {
                                    GarageKeys.Delete();
                                }

                                WebClient ws = new WebClient();
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                string response = ws.DownloadString(github_models);
                                while (response == null) Script.Wait(0);
                                int a = 0;
                                int b = 0;

                                string[] data = response.Split(new char[] { ',' });
                                foreach (string ids in data)
                                {
                                    if (b == 0)
                                    {
                                        string car_ini = $"CAR_{a}";
                                        config.SetValue(car_ini, "model", ids);
                                        config.Save();
                                        b = 1;
                                    }
                                    else
                                    {
                                        if (b == 1)
                                        {
                                            string car_ini = $"CAR_{a}";
                                            config.SetValue(car_ini, "LIVERY", ids);
                                            config.Save();
                                            b = 2;
                                        }
                                        else
                                        {
                                            if (b == 2)
                                            {
                                                string car_ini = $"CAR_{a}";
                                                config.SetValue(car_ini, "COLOR_TYPE", ids);
                                                config.Save();
                                                b = 3;
                                            }
                                            else
                                            {
                                                if (b == 3)
                                                {
                                                    string car_ini = $"CAR_{a}";
                                                    config.SetValue(car_ini, "COLOR_1", ids);
                                                    config.Save();
                                                    b = 4;
                                                }
                                                else
                                                {
                                                    if (b == 4)
                                                    {
                                                        string car_ini = $"CAR_{a}";
                                                        config.SetValue(car_ini, "COLOR_2", ids);
                                                        config.Save();
                                                        b = 0;
                                                        a++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                config.SetValue("SETTINGS", "current_selection", downloaded_response);
                                config.Save();

                                Wait(1000);
                                CreateVehicles(9);
                                if (veh[0] != null)
                                {
                                    prize = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[0].X, position[0].Y, position[0].Z);
                                    GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, prize, 781);
                                }
                                GarageKeys = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, position[12].X, position[12].Y, position[12].Z);
                                GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, GarageKeys, 811);
                                club.Delete();

                                Wait(1000);
                                Function.Call(Hash.DO_SCREEN_FADE_IN, 1000);
                            }
                        }
                    }
                }
            }
        }
    }
}