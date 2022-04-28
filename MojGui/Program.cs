using NStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Terminal.Gui;
using MojGui.Helpers;
using MojGui.Entities;


namespace MojGui
{
    internal class Program
    {
		public static Action running = MainApp;
		static void Main(string[] args)
        {

			if (args.Length > 0 && args.Contains("-usc"))
			{
				Application.UseSystemConsole = true;
			}

			Console.OutputEncoding = System.Text.Encoding.Default;

			while (running != null)
			{
				running.Invoke();
			}
			Application.Shutdown();
		}

        #region KeyDown / KeyPress / KeyUp Demo
        private static void OnKeyDownPressUpDemo()
        {
            var close = new Button("Close");
            close.Clicked += () => { Application.RequestStop(); };
            var container = new Dialog("Lista Zaposlenih", 80, 20, close)
            {
                Title = "Kiza",
                Width = Dim.Fill(),

                Height = Dim.Fill(),
            };

            var list = new List<string>();
            var tableView = new TableView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill()-1,
                Height = Dim.Fill()-2,
            };
            tableView.ColorScheme = Colors.TopLevel;
            container.Add(tableView);

            using (var db = new NorthwindContext())
            {
                var employees = db.Employees.ToList();

                    var converter = new ListToDateTableConverter();

                    tableView.Table = converter.ToDataTable(employees);
            }
                //void KeyDownPressUp(KeyEvent keyEvent, string updown)
                //{
                //    const int ident = -5;
                //    switch (updown)
                //    {
                //        case "Down":
                //        case "Up":
                //        case "Press":
                //            var msg = $"Key{updown,ident}: ";
                //            if ((keyEvent.Key & Key.ShiftMask) != 0)
                //                msg += "Shift ";
                //            if ((keyEvent.Key & Key.CtrlMask) != 0)
                //                msg += "Ctrl ";
                //            if ((keyEvent.Key & Key.AltMask) != 0)
                //                msg += "Alt ";
                //            msg += $"{(((uint)keyEvent.KeyValue & (uint)Key.CharMask) > 26 ? $"{(char)keyEvent.KeyValue}" : $"{keyEvent.Key}")}";
                //            //list.Add (msg);
                //            list.Add(keyEvent.ToString());

                //            break;

                //        default:
                //            if ((keyEvent.Key & Key.ShiftMask) != 0)
                //            {
                //                list.Add($"Key{updown,ident}: Shift ");
                //            }
                //            else if ((keyEvent.Key & Key.CtrlMask) != 0)
                //            {
                //                list.Add($"Key{updown,ident}: Ctrl ");
                //            }
                //            else if ((keyEvent.Key & Key.AltMask) != 0)
                //            {
                //                list.Add($"Key{updown,ident}: Alt ");
                //            }
                //            else
                //            {
                //                list.Add($"Key{updown,ident}: {(((uint)keyEvent.KeyValue & (uint)Key.CharMask) > 26 ? $"{(char)keyEvent.KeyValue}" : $"{keyEvent.Key}")}");
                //            }

                //            break;
                //    }
                //    //listView.MoveDown();
                //}

                //container.KeyDown += (e) => KeyDownPressUp(e.KeyEvent, "Down");
                //container.KeyPress += (e) => KeyDownPressUp(e.KeyEvent, "Press");
                //container.KeyUp += (e) => KeyDownPressUp(e.KeyEvent, "Up");
                Application.Run(container);
        }
        #endregion
        public static Label ml;
        public static MenuBar menu;
        public static CheckBox menuKeysStyle;
        public static CheckBox menuAutoMouseNav;
        private static bool heightAsBuffer = false;
        static void MainApp()
        {
            if (Debugger.IsAttached)
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            Application.Init();
            Application.HeightAsBuffer = heightAsBuffer;
            //ConsoleDriver.Diagnostics = ConsoleDriver.DiagnosticFlags.FramePadding | ConsoleDriver.DiagnosticFlags.FrameRuler;

            var top = Application.Top;

            //Open ();
#if true
            int margin = 3;
            var win = new Window("Hello")
            {
                X = 1,
                Y = 1,

                Width = Dim.Fill() - margin,
                Height = Dim.Fill() - margin
            };
#else
        		var tframe = top.Frame;

        		var win = new Window (new Rect (0, 1, tframe.Width, tframe.Height - 1), "Hello");
#endif
            MenuItemDetails[] menuItems = {
                    new MenuItemDetails ("F_ind", "", null),
                    new MenuItemDetails ("_Replace", "", null),
                    new MenuItemDetails ("_Item1", "", null),
                    new MenuItemDetails ("_Also From Sub Menu", "", null)
                };

            menuItems[0].Action = () => ShowMenuItem(menuItems[0]);
            menuItems[1].Action = () => ShowMenuItem(menuItems[1]);
            menuItems[2].Action = () => ShowMenuItem(menuItems[2]);
            menuItems[3].Action = () => ShowMenuItem(menuItems[3]);

        

            menu = new MenuBar(new MenuBarItem[] {
                    new MenuBarItem ("_File", new MenuItem [] {
                       
                        new MenuItem ("_Quit", "", () => { if (Quit ()) { running = null; top.Running = false; } }, null, null, Key.CtrlMask | Key.Q)
                    }),
                    new MenuBarItem ("Employees", new MenuItem [] {
                new MenuItem ("_List", "", () => OnKeyDownPressUpDemo (), null, null, Key.AltMask | Key.CtrlMask | Key.G),
              
            }),
                    new MenuBarItem ("_About...", "Demonstrates top-level menu item", () =>  MessageBox.ErrorQuery (50, 7, "About Demo", "This is a demo app for gui.cs", "Ok")),
        });




            menuKeysStyle = new CheckBox(3, 25, "UseKeysUpDownAsKeysLeftRight", true);
            menuKeysStyle.Toggled += MenuKeysStyle_Toggled;
            menuAutoMouseNav = new CheckBox(40, 25, "UseMenuAutoNavigation", true);
            menuAutoMouseNav.Toggled += MenuAutoMouseNav_Toggled;

            //ShowEntries(win);

           
           

            var test = new Label(3, 18, "Se iniciará el análisis");
            win.Add(test);
          

            var drag = new Label("Drag: ") { X = 70, Y = 22 };
            var dragText = new TextField("")
            {
                X = Pos.Right(drag),
                Y = Pos.Top(drag),
                Width = 40
            };

            var statusBar = new StatusBar(new StatusItem[] {
                    new StatusItem(Key.F1, "~F1~ Help", () => Help()),
                    new StatusItem(Key.F2, "~F2~ Load", Load),
                    new StatusItem(Key.F3, "~F3~ Save", Save),
                    new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => { if (Quit ()) { running = null; top.Running = false; } }),
                    new StatusItem(Key.Null, Application.Driver.GetType().Name, null)
                });

            win.Add(drag, dragText);

            var bottom = new Label("This should go on the bottom of the same top-level!");
            win.Add(bottom);
            var bottom2 = new Label("This should go on the bottom of another top-level!");
            top.Add(bottom2);

            top.LayoutComplete += (e) =>
            {
                bottom.X = win.X;
                bottom.Y = Pos.Bottom(win) - Pos.Top(win) - margin;
                bottom2.X = Pos.Left(win);
                bottom2.Y = Pos.Bottom(win);
            };

            win.KeyPress += Win_KeyPress;

            top.Add(win);
            //top.Add (menu);
            top.Add(menu, statusBar);
            Application.Run(top);
        }

        //private static void ShowEntries(Window win)
        //{
        //    throw new NotImplementedException();
        //}

        public class MenuItemDetails : MenuItem
        {
            ustring title;
            string help;
            Action action;

            public MenuItemDetails(ustring title, string help, Action action) : base(title, help, action)
            {
                this.title = title;
                this.help = help;
                this.action = action;
            }

            public static MenuItemDetails Instance(MenuItem mi)
            {
                return (MenuItemDetails)mi.GetMenuItem();
            }
        }

        public delegate MenuItem MenuItemDelegate(MenuItemDetails menuItem);

        public static void ShowMenuItem(MenuItem mi)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            MethodInfo minfo = typeof(MenuItemDetails).GetMethod("Instance", flags);
            MenuItemDelegate mid = (MenuItemDelegate)Delegate.CreateDelegate(typeof(MenuItemDelegate), minfo);
            MessageBox.Query(70, 7, mi.Title.ToString(),
                $"{mi.Title.ToString()} selected. Is from submenu: {mi.GetMenuBarItem()}", "Ok");
        }

        static void MenuKeysStyle_Toggled(bool e)
        {
            menu.UseKeysUpDownAsKeysLeftRight = menuKeysStyle.Checked;
        }

        static void MenuAutoMouseNav_Toggled(bool e)
        {
            menu.WantMousePositionReports = menuAutoMouseNav.Checked;
        }

        static void Copy()
        {
            TextField textField = menu.LastFocused as TextField ?? Application.Top.MostFocused as TextField;
            if (textField != null && textField.SelectedLength != 0)
            {
                textField.Copy();
            }
        }

        static void Cut()
        {
            TextField textField = menu.LastFocused as TextField ?? Application.Top.MostFocused as TextField;
            if (textField != null && textField.SelectedLength != 0)
            {
                textField.Cut();
            }
        }

        static void Paste()
        {
            TextField textField = menu.LastFocused as TextField ?? Application.Top.MostFocused as TextField;
            textField?.Paste();
        }

        static void Help()
        {
            MessageBox.Query(50, 7, "Help", "This is a small help\nBe kind.", "Ok");
        }

        static void Load()
        {
            MessageBox.Query(50, 7, "Load", "This is a small load\nBe kind.", "Ok");
        }

        static void Save()
        {
            MessageBox.Query(50, 7, "Save", "This is a small save\nBe kind.", "Ok");
        }
        private static void Win_KeyPress(View.KeyEventEventArgs e)
        {
            switch (ShortcutHelper.GetModifiersKey(e.KeyEvent))
            {
                case Key.CtrlMask | Key.T:
                    if (menu.IsMenuOpen)
                        menu.CloseMenu();
                    else
                        menu.OpenMenu();
                    e.Handled = true;
                    break;
            }
           
        }
        static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
            return n == 0;
        }
    }
  
    }


