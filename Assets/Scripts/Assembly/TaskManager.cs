using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Tambahkan ini untuk mengatur tombol Check

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    BuildTask currentTask;
    public TMP_Text taskText;
    
    // Tambahkan referensi tombol Check agar bisa diaktifkan/dimatikan lewat code
    public Button checkButton; 

    List<PartType> installedParts = new List<PartType>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTask = GameManager.Instance.currentTask;

        if (currentTask == null)
        {
            Debug.LogError("NO TASK FOUND");
            return;
        }

        UpdateTaskUI();
    }

    public void RegisterInstalledPart(PartType part)
    {
        if (installedParts.Contains(part))
            return;

        installedParts.Add(part);
        Debug.Log(part + " Installed");

        UpdateTaskUI();
    }

    public void RemoveInstalledPart(PartType part)
    {
        if (installedParts.Contains(part))
        {
            installedParts.Remove(part);
            Debug.Log(part + " Removed");

            UpdateTaskUI();
        }
    }

    // FUNGSI BARU: Mengecek apakah tombol Check boleh aktif atau tidak
    bool IsMinimalOnePartInstalled()
    {
        // Mengecek dari daftar part yang diminta di BuildTask, apakah ada yang sudah terpasang
        foreach (PartType requiredPart in currentTask.requiredParts)
        {
            if (installedParts.Contains(requiredPart))
            {
                return true; // Ketemu minimal 1 part yang cocok!
            }
        }
        return false; // Sama sekali belum ada part yang dipasang
    }

    // Pemicu utama saat tombol "Check" diklik atau saat timer habis
    public void CheckCurrentTask()
    {
        // Kondisi 1: Jika sama sekali belum ada part yang selesai, tombol tidak berefek (langsung return)
        if (!IsMinimalOnePartInstalled())
        {
            Debug.Log("Tombol Check Terkunci: Pasang minimal 1 part dulu!");
            return;
        }

        // Kondisi 2: Minimal 1 task selesai, jalankan pembagian uang dan pindah scene
        CalculateFinalRewards();
    }

    void CalculateFinalRewards()
    {
        // Hentikan timer terlebih dahulu agar angkanya mengunci
        float sisaWaktu = 0f;
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.timerIsRunning = false;
            sisaWaktu = TimeManager.Instance.GetRemainingTime(); // Ambil sisa detik terakhir
        }

        int totalUangDidapat = 0;
        bool semuaPartLengkap = true;

        // 1. Hitung uang berdasarkan part yang BERHASIL terpasang
        foreach (PartType requiredPart in currentTask.requiredParts)
        {
            if (installedParts.Contains(requiredPart))
            {
                // Tambah uang sesuai harga masing-masing part yang kamu tentukan
                if (requiredPart == PartType.CPU) totalUangDidapat += 150;
                else if (requiredPart == PartType.RAM) totalUangDidapat += 100;
                else if (requiredPart == PartType.GPU) totalUangDidapat += 100;
                else if (requiredPart == PartType.PSU) totalUangDidapat += 50; // Jaga-jaga jika ada PSU
            }
            else
            {
                semuaPartLengkap = false; // Ada part yang bolong
            }
        }

        // 2. Hitung Bonus berdasarkan sisa waktu (HANYA JIKA semua part terpasang lengkap)
        if (semuaPartLengkap && sisaWaktu > 0)
        {
            // Waktu awal 60 detik. 
            // Kurang dari 30 detik artinya sisa waktu di timer masih DI ATAS 30 detik.
            // Kurang dari 45 detik artinya sisa waktu di timer masih DI ATAS 15 detik.
            if (sisaWaktu >= 30f)
            {
                totalUangDidapat += 50;
                Debug.Log("Bonus Kecepatan Kilat (+50): Selesai dalam " + (60f - sisaWaktu) + " detik!");
            }
            else if (sisaWaktu >= 15f)
            {
                totalUangDidapat += 25;
                Debug.Log("Bonus Kecepatan Menengah (+25): Selesai dalam " + (60f - sisaWaktu) + " detik!");
            }
        }

        // 3. Kirim total uang ke EconomyManager/GameManager
        EconomyManager.Instance.AddMoney(totalUangDidapat);

        // 4. Reset & Pindah Scene
        BuildManager.Instance.ClearAllParts();
        installedParts.Clear();
        GameManager.Instance.returningFromAssembly = true;
        SceneManager.LoadScene("DialogueScene");
    }

    void UpdateTaskUI()
    {
        string display = currentTask.taskName + "\n\n";

        foreach (PartType requiredPart in currentTask.requiredParts)
        {
            bool installed = installedParts.Contains(requiredPart);
            string check = installed ? "[X] " : "[ ] ";
            display += check + requiredPart + "\n";
        }

        taskText.text = display;

        // AUTOMATISASI UI: Menghidupkan/mematikan tombol secara visual (interactable) di Unity
        if (checkButton != null)
        {
            checkButton.interactable = IsMinimalOnePartInstalled();
        }
    }
}