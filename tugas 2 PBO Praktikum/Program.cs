using System;

public interface Kemampuan
{
    void Aktifkan(Robot robot, Robot target = null);
    int Cooldown { get; }
    string Deskripsi { get; } // Add a description property
}

public abstract class Robot
{
    public string nama;
    public int energi;
    public int armor;
    public int serangan;

    public Robot(string nama, int energi, int armor, int serangan)
    {
        this.nama = nama;
        this.energi = energi;
        this.armor = armor;
        this.serangan = serangan;
    }

    public abstract void Serang(Robot target);
    public abstract void GunakanKemampuan(Kemampuan kemampuan, Robot target = null);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {nama}");
        Console.WriteLine($"Energi: {energi}");
        Console.WriteLine($"Armor: {armor}");
        Console.WriteLine($"Serangan: {serangan}");
    }

    public int Energi
    {
        get { return energi; }
        set { energi = value; }
    }
}

public class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public override void Serang(Robot target)
    {
        target.Energi -= serangan;
        Console.WriteLine($"{nama} menyerang {target.nama} dan menyebabkan {serangan} kerusakan.");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target = null)
    {
        kemampuan.Aktifkan(this, target);
    }
}

public class BosRobot : Robot
{
    private int pertahanan;
    private bool hasUsedSpecialAttack = false; // Track if special ability has been used

    public BosRobot(string nama, int energi, int armor, int serangan, int pertahanan)
        : base(nama, energi, armor, serangan)
    {
        this.pertahanan = pertahanan;
    }

    public override void Serang(Robot target)
    {
        int damage = Math.Max(0, serangan - target.armor);
        target.Energi -= damage;
        Console.WriteLine($"{nama} menyerang {target.nama} dan menyebabkan {damage} kerusakan.");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target = null)
    {
        kemampuan.Aktifkan(this, target);
    }

    public void SeranganSpesial(Robot target)
    {
        if (energi <= 20 && !hasUsedSpecialAttack)
        {
            energi += 20; // Heal 20
            Console.WriteLine($"{nama} menggunakan kemampuan spesial dan menyembuhkan diri hingga {energi} energi.");
            target.Energi -= 20; // Deal 20 damage to target
            Console.WriteLine($"{nama} menyerang {target.nama} dengan serangan spesial dan menyebabkan 20 kerusakan.");
            hasUsedSpecialAttack = true; // Mark special ability as used
        }
    }
}

public class Perbaikan : Kemampuan
{
    public int cooldown;
    public int lastUsed;

    public Perbaikan(int cooldown)
    {
        this.cooldown = cooldown;
        this.lastUsed = cooldown; // Awalnya cooldown siap digunakan
    }

    public void Aktifkan(Robot robot, Robot target = null)
    {
        if (robot.Energi > 20)
        {
            Console.WriteLine($"{robot.nama} tidak dapat menggunakan kemampuan Perbaikan. Energi lebih dari 20.");
            return;
        }

        if (lastUsed >= cooldown)
        {
            robot.Energi += 20;
            Console.WriteLine($"{robot.nama} menggunakan kemampuan Perbaikan, energi sekarang: {robot.Energi}");
            lastUsed = 0; // Reset cooldown
        }
        else
        {
            Console.WriteLine($"{robot.nama} kemampuan Perbaikan dalam cooldown, melakukan menyerang.");
        }
    }

    public int Cooldown => cooldown;

    public string Deskripsi => "Mengembalikan 20 energi. Dapat digunakan jika energi kurang dari atau sama dengan 20.";

    public void CooldownTick()
    {
        if (lastUsed < cooldown)
        {
            lastUsed++;
        }
    }
}

public class SeranganListrik : Kemampuan
{
    public int cooldown;
    public int lastUsed;

    public SeranganListrik(int cooldown)
    {
        this.cooldown = cooldown;
        this.lastUsed = cooldown; // Awalnya cooldown siap digunakan
    }

    public void Aktifkan(Robot robot, Robot target)
    {
        if (lastUsed >= cooldown)
        {
            Console.WriteLine($"{robot.nama} menggunakan kemampuan Serangan Listrik.");
            lastUsed = 0; // Reset cooldown
            // Mengabaikan serangan bos robot di giliran ini
        }
        else
        {
            Console.WriteLine($"{robot.nama} kemampuan Serangan Listrik dalam cooldown, melakukan menyerang.");
        }
    }

    public int Cooldown => cooldown;

    public string Deskripsi => "Menyetrum musuh, membuatnya tidak bisa menyerang di giliran ini.";

    public void CooldownTick()
    {
        if (lastUsed < cooldown)
        {
            lastUsed++;
        }
    }
}

public class SeranganPlasma : Kemampuan
{
    public int cooldown;
    public int lastUsed;

    public SeranganPlasma(int cooldown)
    {
        this.cooldown = cooldown;
        this.lastUsed = cooldown; // Awalnya cooldown siap digunakan
    }

    public void Aktifkan(Robot robot, Robot target)
    {
        if (lastUsed >= cooldown)
        {
            int damage = 50; // Plasma deals 50 damage
            target.Energi -= damage;
            Console.WriteLine($"{robot.nama} menggunakan kemampuan Serangan Plasma dan menyebabkan {damage} kerusakan kepada {target.nama}.");
            lastUsed = 0; // Reset cooldown
        }
        else
        {
            Console.WriteLine($"{robot.nama} kemampuan Serangan Plasma dalam cooldown, melakukan menyerang.");
        }
    }

    public int Cooldown => cooldown;

    public string Deskripsi => "Menyerang dengan plasma dan menyebabkan 50 kerusakan ke musuh.";

    public void CooldownTick()
    {
        if (lastUsed < cooldown)
        {
            lastUsed++;
        }
    }
}

public class PertahananSuper : Kemampuan
{
    public int cooldown;
    public int lastUsed;

    public PertahananSuper(int cooldown)
    {
        this.cooldown = cooldown;
        this.lastUsed = cooldown; // Awalnya cooldown siap digunakan
    }

    public void Aktifkan(Robot robot, Robot target = null)
    {
        if (lastUsed >= cooldown)
        {
            robot.armor += 10; // Meningkatkan armor sementara
            Console.WriteLine($"{robot.nama} menggunakan kemampuan Pertahanan Super, armor sekarang: {robot.armor}");
            lastUsed = 0; // Reset cooldown
        }
        else
        {
            Console.WriteLine($"{robot.nama} kemampuan Pertahanan Super dalam cooldown, melakukan menyerang.");
        }
    }

    public int Cooldown => cooldown;

    public string Deskripsi => "Meningkatkan armor sebesar 10 untuk giliran ini.";

    public void CooldownTick()
    {
        if (lastUsed < cooldown)
        {
            lastUsed++;
        }
    }
}

public class SimulatorPertarungan
{
    public static void Main(string[] args)
    {
        BosRobot bosRobot = new BosRobot("Ambatron Mega", 100, 15, 25, 10);
        RobotBiasa robot1 = new RobotBiasa("Arisu cnnuy", 80, 10, 20);
        Perbaikan perbaikan = new Perbaikan(2);
        SeranganListrik seranganListrik = new SeranganListrik(3);
        SeranganPlasma seranganPlasma = new SeranganPlasma(4);
        PertahananSuper pertahananSuper = new PertahananSuper(5);

        while (bosRobot.energi > 0 && robot1.energi > 0)
        {
            Console.WriteLine("\n--- Giliran Baru ---");
            robot1.CetakInformasi();
            bosRobot.CetakInformasi();

            // Menyediakan pilihan untuk melanjutkan turn atau mengakhiri permainan
            Console.WriteLine("1. Serang");
            Console.WriteLine("2. Gunakan Kemampuan");
            Console.WriteLine("3. Akhiri Permainan");
            Console.WriteLine("Pilih opsi (1-3): ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    // Simulasi serangan dari robot biasa ke bos robot
                    robot1.Serang(bosRobot);
                    break;

                case "2":
                    Console.WriteLine("Pilih kemampuan:");
                    Console.WriteLine("1. Perbaikan");
                    Console.WriteLine("2. Serangan Listrik");
                    Console.WriteLine("3. Serangan Plasma");
                    Console.WriteLine("4. Pertahanan Super");

                    // Display ability descriptions
                    Console.WriteLine("Kemampuan:");
                    Console.WriteLine($"1. Perbaikan - {perbaikan.Deskripsi} (Cooldown: {perbaikan.Cooldown})");
                    Console.WriteLine($"2. Serangan Listrik - {seranganListrik.Deskripsi} (Cooldown: {seranganListrik.Cooldown})");
                    Console.WriteLine($"3. Serangan Plasma - {seranganPlasma.Deskripsi} (Cooldown: {seranganPlasma.Cooldown})");
                    Console.WriteLine($"4. Pertahanan Super - {pertahananSuper.Deskripsi} (Cooldown: {pertahananSuper.Cooldown})");

                    string abilityChoice = Console.ReadLine();

                    Robot target = bosRobot; // Target adalah bos robot

                    switch (abilityChoice)
                    {
                        case "1":
                            robot1.GunakanKemampuan(perbaikan);
                            break;
                        case "2":
                            robot1.GunakanKemampuan(seranganListrik, target);
                            break;
                        case "3":
                            robot1.GunakanKemampuan(seranganPlasma, target);
                            break;
                        case "4":
                            robot1.GunakanKemampuan(pertahananSuper);
                            break;
                        default:
                            Console.WriteLine("Kemampuan tidak valid. Melanjutkan serangan.");
                            break;
                    }
                    break;

                case "3":
                    Console.WriteLine("Permainan telah diakhiri. Terima kasih telah bermain!");
                    return; // Mengakhiri permainan

                default:
                    Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                    continue; // Kembali ke awal loop
            }

            // Check if the boss robot can use special ability
            if (bosRobot.energi <= 20)
            {
                bosRobot.SeranganSpesial(robot1);
            }
            else if (seranganListrik.lastUsed < seranganListrik.cooldown)
            {
                Console.WriteLine($"{bosRobot.nama} tidak dapat menyerang karena terkena Serangan Listrik!");
            }
            else
            {
                bosRobot.Serang(robot1);
            }

            // Menjalankan cooldown untuk setiap kemampuan
            perbaikan.CooldownTick();
            seranganListrik.CooldownTick();
            seranganPlasma.CooldownTick();
            pertahananSuper.CooldownTick();

            // Mengecek apakah salah satu robot kalah
            if (bosRobot.energi <= 0)
            {
                Console.WriteLine($"{bosRobot.nama} telah kalah!");
                break; // Keluar dari loop jika bos robot kalah
            }

            if (robot1.energi <= 0)
            {
                Console.WriteLine($"{robot1.nama} telah kalah!");
                break; // Keluar dari loop jika robot biasa kalah
            }
        }
    }
}