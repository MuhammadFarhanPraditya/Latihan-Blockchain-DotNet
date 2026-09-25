// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract PenyimpananSederhana {
    uint256 public angkaTersimpan;

    // Fungsi untuk mengubah data (Write Operation - Butuh Gas/Biaya)
    function simpanAngka(uint256 _angkaBaru) public {
        angkaTersimpan = _angkaBaru;
    }

    // Fungsi untuk membaca data (Read Operation - Gratis)
    function bacaAngka() public view returns (uint256) {
        return angkaTersimpan;
    }
}