/*
    Example made by: Slashury
    Original author: justalemon
    Original author's Discord Server: https://discord.gg/Cf6sspj
    Links for the original mod: https://www.gta5-mods.com/tools/lemonui or https://github.com/LemonUIbyLemon/LemonUI
    I will not be giving support to anyone about further questions, if you have any questions ask it in Lemon's server which is above.
    Links so you can learn more about LemonUI: https://github.com/LemonUIbyLemon/LemonUI/wiki/Quick-Start & https://github.com/LemonUIbyLemon/LemonUI/wiki/Upgrading-from-NativeUI
*/


using System;
using System.Windows.Forms; //for scripting, you will need this library.
using GTA; //for scripting, you will need this library.
using GTA.Math; //for scripting, you will need this library.
using GTA.Native; //for scripting, you will need this library.
using GTA.UI; //for scripting, you will need this library.
using LemonUI; //You need LemonUI so the MenuExample will work properly (not a requirement for basic scripting which does not require menu systems).


public class Main : Script
{
    ObjectPool pool;
    LemonUI.Menus.NativeItem basicItem;
    LemonUI.Menus.NativeMenu menu;
    LemonUI.Menus.NativeCheckboxItem checkboxItem;
    LemonUI.Menus.NativeListItem ListItems;
    LemonUI.Menus.NativeSliderItem SliderItem;

    public Main()
    {
        Setup(); //This is where we call our void Setup().
        Tick += OnTick;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp; // We need OnKeyUp, so we can make the Menu visible with a button pressed ingame, for this example, the mod will use F3.
    }
    void Setup()
    {
        pool = new ObjectPool(); //Object Pool stores all of your UI changes. This line stores your Menu.
        menu = new LemonUI.Menus.NativeMenu("Menu Example", "Choose an Option" /*(<-Subtitle)*/); //With this line of code, LemonUI allows you to create Rockstar-like menus with the NativeMenu class. (Such as Director Mode menu, Interaction Menu, when pressing M)
        pool.Add(menu); //This adds the menu into an Object Pool.

        basicItem = new LemonUI.Menus.NativeItem("Basic Item", "This is the description of a basic Item"); //This is how you create a simple Item to your menu. "Basic Item" will be the title, and the second will be the description.
        menu.Add(basicItem); //This line of code adds the basicItem to your Menu.

        checkboxItem = new LemonUI.Menus.NativeCheckboxItem("CheckBox Test", "This is the description of a checkbox Item"/*, true); It can be set true by default */ ); //This is how you create a simple checkbox Item to your menu. It can be either toggled on or off.
        menu.Add(checkboxItem); //This line of code adds the CheckboxItem to your Menu.

        ListItems = new LemonUI.Menus.NativeListItem<int>("ListItem Test", "This is the description of a list Item", 1, 2, 3, 4, 5); //This is how you create a simple ListItem to your menu. it allows you to scroll between 1 and 5.
        menu.Add(ListItems); //This line of code adds the certain Item to your Menu.

        SliderItem = new LemonUI.Menus.NativeSliderItem("SliderItem Test", "This is the description of a slider Item"); //This is how you crate a simple Slider Item. You can move from Left to Right to change a numeric value. 
        menu.Add(SliderItem); //This line of code adds the SliderItem to your Menu.
    }


    private void OnTick(object sender, EventArgs e)
    {
        pool.Process(); //This line of code it makes sure, that is able to detect changes and draw the UI elements.
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {

    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F3) //This block what essentially does is, it will check if you pressed F3 ingame, and if so, then it will make your menu visible (menu.Visible = true)
        {
            menu.Visible = true;
        }

    }

}