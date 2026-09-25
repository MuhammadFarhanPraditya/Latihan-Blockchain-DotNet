using System;
using System.IO;
using System.Text.Json;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts; // [BARU] Wajib untuk interaksi dengan Contract

try
{
    // 1. Setup Akun
    string privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80"; 
    var account = new Account(privateKey);
    var web3 = new Web3(account, "http://127.0.0.1:8545");
    Console.WriteLine($"[1] Terhubung dengan akun dompet: {account.Address}");

    // 2. Baca ABI (Bytecode sudah tidak dibutuhkan)
    string artifactPath = Path.Combine("..", "artifacts", "contracts", "PenyimpananSederhana.sol", "PenyimpananSederhana.json");
    string jsonContent = File.ReadAllText(artifactPath);
    using JsonDocument doc = JsonDocument.Parse(jsonContent);
    string abi = doc.RootElement.GetProperty("abi").ToString();

    // MASUKKAN ALAMAT CONTRACT DARI HASIL DEPLOY SEBELUMNYA DI SINI!
    string contractAddress = "0x5fbdb2315678afecb367f032d93f642f64180aa3";
    
    // Inisialisasi instance contract
    var contract = web3.Eth.GetContract(abi, contractAddress);
    Console.WriteLine($"[2] Terhubung ke Smart Contract di: {contractAddress}");

    // ==========================================
    // SKENARIO 1: MEMBACA DATA (READ - GRATIS)
    // ==========================================
    Console.WriteLine("\n[3] Membaca nilai awal dari blockchain...");
    
    // Kita panggil fungsi "bacaAngka" (wajib sama dengan nama fungsi di Solidity)
    var bacaFunction = contract.GetFunction("bacaAngka");
    
    // CallAsync untuk operasi Read (tidak mengubah state, tidak butuh gas)
    var nilaiAwal = await bacaFunction.CallAsync<int>(); 
    Console.WriteLine($"--> Nilai tersimpan saat ini: {nilaiAwal}");


    // ==========================================
    // SKENARIO 2: MENGUBAH DATA (WRITE - BUTUH GAS)
    // ==========================================
    int angkaBaru = 99; // Angka yang akan kita simpan
    Console.WriteLine($"\n[4] Menyimpan angka baru ({angkaBaru}) ke blockchain...");
    
    var simpanFunction = contract.GetFunction("simpanAngka");
    
    // SendTransactionAndWaitForReceiptAsync untuk operasi Write (mengubah state, butuh gas)
    // Argumen di dalam fungsi dikirimkan sebagai parameter terakhir
    var receipt = await simpanFunction.SendTransactionAndWaitForReceiptAsync(account.Address, new HexBigInteger(3000000), new HexBigInteger(0), null, angkaBaru);
    Console.WriteLine($"--> Transaksi sukses. Gas terpakai: {receipt.GasUsed.Value}");


    // ==========================================
    // SKENARIO 3: MEMBACA KEMBALI SETELAH UBAH
    // ==========================================
    Console.WriteLine("\n[5] Membaca ulang nilai dari blockchain...");
    var nilaiAkhir = await bacaFunction.CallAsync<int>();
    Console.WriteLine($"--> Nilai tersimpan sekarang: {nilaiAkhir}");

}
catch (Exception ex)
{
    Console.WriteLine($"Terjadi kesalahan sistem:");
    Console.WriteLine(ex.Message);
}