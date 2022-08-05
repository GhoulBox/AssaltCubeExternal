using System.Diagnostics;

namespace AssualtCubeExternal
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Assault Cube Offsets
        public const int LocalPlayerOffset = 0x10F4F4;
        public const int HealthOffset = 0xF8;

        private void Form1_Load(object sender, EventArgs e)
        {
                
        }
        public static int GetModuleBaseAddress(Process process, string moduleName) => (int)process.Modules.Cast<ProcessModule>().First(m => m.ModuleName.Equals(moduleName)).BaseAddress;
        private void button1_Click(object sender, EventArgs e)
        {
            Process[] gameProc = Process.GetProcessesByName("ac_client");
            if (gameProc.Length > 0)
            {
                Memory.ProcessHandle = Memory.OpenProcess(0x0008 | 0x0010 | 0x0020, false, gameProc[0].Id);

                //We get the address or location of where ac_client.exe is inside the Assault Cube game.
                //Processes (.exes) don't start at 0 because there is windows header stuff that is there so we have to find where the game's memory starts at
                int gameBase = GetModuleBaseAddress(gameProc[0], "ac_client.exe");

                //Get our local player address. This is the location of where our local player is
                int LocalPlayer = Memory.ReadMemory<int>(gameBase + LocalPlayerOffset);

                //Just a test read, reads the player's current HP. We don't need it back you can add "Current HP: 100" Form1 if you want with this.
                int Health = Memory.ReadMemory<int>(LocalPlayer + HealthOffset);


                //Convert the textBox1's Text into an int. The value 'userhealth' will contain the health the user put in as an int.
                //We put this in an if statement incase the user inputs letters and we can't convert letters into numbers :p
                if (int.TryParse(textBox1.Text, out int userHealth))
                {
                    Memory.WriteMemory<int>(LocalPlayer + HealthOffset, userHealth);
                    MessageBox.Show($"Set Health from {Health} to {userHealth}");
                }
                else
                {
                    MessageBox.Show("uh i don't think that's a number :p");
                }
                    
                


            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made By GhoulBox#1000");
        }

    }
}