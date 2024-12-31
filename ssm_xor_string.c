#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "ssm_xor_string.h"

// Function to convert a hex string to bytes
static int hex_to_bytes(const char *hex, uint8_t *bytes, size_t length) {
    for (size_t i = 0; i < length; i++) {
        if (sscanf(hex + 2 * i, "%2hhx", &bytes[i]) != 1) {
            return -1; // Invalid hex character
        }
    }
    return 0;
}

// Function to convert bytes to a hex string
static void bytes_to_hex(const uint8_t *bytes, size_t length, char *hex) {
    for (size_t i = 0; i < length; i++) {
        sprintf(hex + 2 * i, "%02X", bytes[i]);
    }
}

// XOR a hex string with a 16-bit key
int xor_hex_string_with_key(const char *hex_string, uint16_t xor_key, char **output_hex) {
    size_t hex_len = strlen(hex_string);
    if (hex_len % 2 != 0) {
        fprintf(stderr, "Invalid hex string length.\n");
        return -1;
    }

    size_t byte_len = hex_len / 2;
    uint8_t *bytes = malloc(byte_len);
    if (!bytes) {
        perror("Memory allocation failed");
        return -1;
    }

    if (hex_to_bytes(hex_string, bytes, byte_len) != 0) {
        fprintf(stderr, "Invalid hex string format.\n");
        free(bytes);
        return -1;
    }

    // XOR each byte with the appropriate byte of the 16-bit key
    for (size_t i = 0; i < byte_len; i++) {
        bytes[i] ^= ((i % 2 == 0) ? (xor_key & 0xFF) : ((xor_key >> 8) & 0xFF));
    }

    // Allocate memory for the output hex string
    *output_hex = malloc(hex_len + 1); // Include space for null-terminator
    if (!*output_hex) {
        perror("Memory allocation failed");
        free(bytes);
        return -1;
    }

    // Convert XORed bytes back to a hex string
    bytes_to_hex(bytes, byte_len, *output_hex);
    (*output_hex)[hex_len] = '\0'; // Null-terminate the output string

    free(bytes);
    return 0;
}


// CRC16-Modbus calculation
uint16_t calculate_crc16_modbus(const uint8_t *data, size_t length) {
    uint16_t crc = 0xFFFF;
    for (size_t i = 0; i < length; i++) {
        crc ^= data[i];
        for (int j = 0; j < 8; j++) {
            if (crc & 0x0001) {
                crc = (crc >> 1) ^ 0xA001;
            } else {
                crc >>= 1;
            }
        }
    }
    return crc;
}

char *create_hex_string(uint16_t xor_key, uint8_t encryption_mode, uint8_t connection_mode, uint16_t packet_subject,const char *data_packet)
{
    // Fixed fields
    uint16_t header = 0x7377;
    uint32_t reserved = 0xFFFFFFFF;
    char * output_hex = NULL;

    // Data packet length
    size_t data_packet_length = strlen(data_packet) / 2; // Each byte is 2 hex characters

    // Total size: fixed fields (8 bytes) + data packet + reserved (4 bytes) + CRC (2 bytes)
    size_t total_length = 2 + 2 + 1 + 1 + 2 + data_packet_length + 4 + 2;

    // Allocate memory for binary data
    uint8_t * binary_data = malloc(total_length);
    if (!binary_data) {
        perror("Memory allocation failed");
        return NULL;
    }

    // Populate binary data
    size_t offset = 0;

    // Header
    binary_data[offset++] = (header >> 8) & 0xFF;
    binary_data[offset++] = header & 0xFF;

    // XOR Key
    binary_data[offset++] = (xor_key >> 8) & 0xFF;
    binary_data[offset++] = xor_key & 0xFF;

    // Encryption Mode
    binary_data[offset++] = encryption_mode ^ ( xor_key & 0xFF);

    // Connection Mode
    binary_data[offset++] = connection_mode ^ ((xor_key >> 8) & 0xFF);

    // Packet Subject
    binary_data[offset++] = ((packet_subject >> 8) & 0xFF) ^ ( xor_key & 0xFF);
    binary_data[offset++] = (packet_subject & 0xFF) ^ ((xor_key >> 8) & 0xFF);

    xor_hex_string_with_key(data_packet, xor_key, & output_hex);
    // Data Packet
    for (size_t i = 0; i < data_packet_length; i++) {
        sscanf(output_hex + 2 * i, "%2hhx", & binary_data[offset++]);
    }

    // Reserved
    binary_data[offset++] = ((reserved >> 24) & 0xFF) ^ ( xor_key & 0xFF);
    binary_data[offset++] = ((reserved >> 16) & 0xFF) ^ ((xor_key >> 8) & 0xFF);
    binary_data[offset++] = ((reserved >> 8) & 0xFF) ^ ( xor_key & 0xFF);
    binary_data[offset++] = (reserved & 0xFF) ^ ((xor_key >> 8) & 0xFF);

    // Calculate CRC
    //binary_data+2 : to avoid XOR Header
    uint16_t crc = calculate_crc16_modbus(binary_data, total_length - 2);
    binary_data[offset++] = crc & 0xFF; // Low byte
    binary_data[offset++] = (crc >> 8) & 0xFF; // High byte

    // Allocate memory for hex string
    char * hex_string = malloc(total_length * 2 + 1);
    if (!hex_string) {
        perror("Memory allocation failed");
        free(binary_data);
        return NULL;
    }

    // Convert binary data to hex string
    for (size_t i = 0; i < total_length; i++) {
        sprintf(hex_string + 2 * i, "%02X", binary_data[i]);
    }

    // Null-terminate the hex string
    hex_string[total_length * 2] = '\0';

    free(binary_data);
    return hex_string;
}

/*

int main() {
    // Test parameters
    uint8_t encryption_mode = 0x01; // Example: Encryption Mode = 0x03
    uint8_t connection_mode = 0x02; // Example: Connection Mode = 0x01
    uint16_t xor_key = 0x0507; // Example: XOR Key = 0x0507
    uint16_t packet_subject = 0x01; // Example: Packet Subject = 0x0809
    const char * data_packet = "112233445566"; // Example: Data Packet = "112233445566"

    // Create hex string
    char * hex_string = create_hex_string(xor_key, encryption_mode, connection_mode, packet_subject, data_packet);
    if (hex_string) {
        printf("Generated Hex String: %s\n", hex_string);
        free(hex_string);
    } else {
        printf("Failed to create hex string.\n");
    }

    return 0;
}
*/
char *create_SendDataToServer_string(uint8_t data_packet_length, uint32_t board_id,
									 uint16_t av[4], uint16_t ac[4], uint8_t dig[4],
									 uint16_t res[4], uint8_t ip[4], uint8_t led1_status,
									 uint8_t led2_status, uint8_t relay_status,
									 uint8_t rgb[3], uint8_t status, uint8_t day,
									 uint8_t month, uint8_t year, uint8_t hour,
									 uint8_t minute, uint8_t second)
{
    // Calculate total binary data length
    size_t binary_length = 1 + 4 + (4 * 2) + (4 * 2) + 4 + (4 * 2) + 4 + 1 + 1 +
                           1 + 3 + 1 + 1 + 1 + 1 + 1 + 1 + 1; // Fixed length fields

    // Allocate memory for binary data
    uint8_t *binary_data = malloc(binary_length);
    if (!binary_data) {
        perror("Memory allocation failed");
        return NULL;
    }

    // Populate binary data
    size_t offset = 0;

    binary_data[offset++] = data_packet_length;

    binary_data[offset++] = (board_id >> 24) & 0xFF;
    binary_data[offset++] = (board_id >> 16) & 0xFF;
    binary_data[offset++] = (board_id >> 8) & 0xFF;
    binary_data[offset++] = board_id & 0xFF;

    for (int i = 0; i < 4; i++) {
        binary_data[offset++] = (av[i] >> 8) & 0xFF;
        binary_data[offset++] = av[i] & 0xFF;
    }

    for (int i = 0; i < 4; i++) {
        binary_data[offset++] = (ac[i] >> 8) & 0xFF;
        binary_data[offset++] = ac[i] & 0xFF;
    }

    for (int i = 0; i < 4; i++) {
        binary_data[offset++] = dig[i];
    }

    for (int i = 0; i < 4; i++) {
        binary_data[offset++] = (res[i] >> 8) & 0xFF;
        binary_data[offset++] = res[i] & 0xFF;
    }

    for (int i = 0; i < 4; i++) {
        binary_data[offset++] = ip[i];
    }

    binary_data[offset++] = led1_status;
    binary_data[offset++] = led2_status;
    binary_data[offset++] = relay_status;

    for (int i = 0; i < 3; i++) {
        binary_data[offset++] = rgb[i];
    }

    binary_data[offset++] = status;
    binary_data[offset++] = day;
    binary_data[offset++] = month;
    binary_data[offset++] = year;
    binary_data[offset++] = hour;
    binary_data[offset++] = minute;
    binary_data[offset++] = second;

    // Allocate memory for hex string
    char *hex_string = malloc(binary_length * 2 + 1);
    if (!hex_string) {
        perror("Memory allocation failed");
        free(binary_data);
        return NULL;
    }

    // Convert binary data to hex string
    for (size_t i = 0; i < binary_length; i++) {
        sprintf(hex_string + 2 * i, "%02X", binary_data[i]);
    }

    // Null-terminate the hex string
    hex_string[binary_length * 2] = '\0';

    free(binary_data);
    return hex_string;
}


char *create_SetDefaultSetting_string(uint8_t data_packet_length, uint32_t board_id,
								   uint8_t serial_no[8], uint8_t server_ip[4],
								   uint16_t data_port, uint16_t config_port,
								   uint16_t command_port, uint8_t client_ip[4],
								   uint8_t subnet_mask[4], uint8_t gateway[4],
								   uint8_t dhcp_status, uint8_t wifi_ip[4],
								   uint8_t ssid_length, const char *ssid,
								   uint8_t wifi_pass_len, const char *wifi_password,
								   uint8_t online_cycle_time, uint8_t offline_cycle_time) {
	// Ensure SSID length does not exceed 20
	if (ssid_length > 20) ssid_length = 20;

	// Ensure WiFi password length does not exceed 20
	size_t wifi_password_length = strlen(wifi_password);
	if (wifi_password_length > 20) wifi_password_length = 20;

	// Calculate total binary data length
	size_t binary_length = 1 + 4 + 8 + 4 + 2 + 2 + 2 + 4 + 4 + 4 + 1 + 4 + 20 + 1 + 20 + 1 + 1;

	// Allocate memory for binary data
	uint8_t *binary_data = malloc(binary_length);
	if (!binary_data) {
		perror("Memory allocation failed");
		return NULL;
	}

	// Populate binary data
	size_t offset = 0;

	binary_data[offset++] = data_packet_length;

	binary_data[offset++] = (board_id >> 24) & 0xFF;
	binary_data[offset++] = (board_id >> 16) & 0xFF;
	binary_data[offset++] = (board_id >> 8) & 0xFF;
	binary_data[offset++] = board_id & 0xFF;

	memcpy(&binary_data[offset], serial_no, 8);
	offset += 8;

	memcpy(&binary_data[offset], server_ip, 4);
	offset += 4;

	binary_data[offset++] = (data_port >> 8) & 0xFF;
	binary_data[offset++] = data_port & 0xFF;

	binary_data[offset++] = (config_port >> 8) & 0xFF;
	binary_data[offset++] = config_port & 0xFF;

	binary_data[offset++] = (command_port >> 8) & 0xFF;
	binary_data[offset++] = command_port & 0xFF;

	memcpy(&binary_data[offset], client_ip, 4);
	offset += 4;

	memcpy(&binary_data[offset], subnet_mask, 4);
	offset += 4;

	memcpy(&binary_data[offset], gateway, 4);
	offset += 4;

	binary_data[offset++] = dhcp_status;

	memcpy(&binary_data[offset], wifi_ip, 4);
	offset += 4;

	binary_data[offset++] = ssid_length; // Add SSID length as 1 byte

	memset(&binary_data[offset], 0, 20); // Fill SSID field with zeroes
	memcpy(&binary_data[offset], ssid, ssid_length);
	offset += 20;

	binary_data[offset++] = wifi_pass_len;

	memset(&binary_data[offset], 0, 20); // Fill Wifi Password field with zeroes
	memcpy(&binary_data[offset], wifi_password, wifi_password_length);
	offset += 20;

	binary_data[offset++] = online_cycle_time;
	binary_data[offset++] = offline_cycle_time;

	// Allocate memory for hex string
	char *hex_string = malloc(binary_length * 2 + 1);
	if (!hex_string) {
		perror("Memory allocation failed");
		free(binary_data);
		return NULL;
	}

	// Convert binary data to hex string
	for (size_t i = 0; i < binary_length; i++) {
		sprintf(hex_string + 2 * i, "%02X", binary_data[i]);
	}

	// Null-terminate the hex string
	hex_string[binary_length * 2] = '\0';

	free(binary_data);
	return hex_string;
}


char *create_SendCommandToClient_string(uint8_t data_packet_length, uint32_t board_id,
                                     uint8_t red_rgb, uint8_t green_rgb, uint8_t blue_rgb,
                                     uint8_t led1_status, uint8_t led2_status, uint8_t relay_status,
                                     uint8_t buzzer_status, uint8_t buzzer_code, uint8_t buzzer_timeout) {
    // Calculate total binary data length
    size_t binary_length = 1 + 4 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5; // Data packet length + fields

    // Allocate memory for binary data
    uint8_t *binary_data = malloc(binary_length);
    if (!binary_data) {
        perror("Memory allocation failed");
        return NULL;
    }

    // Populate binary data
    size_t offset = 0;

    binary_data[offset++] = data_packet_length;

    binary_data[offset++] = (board_id >> 24) & 0xFF;
    binary_data[offset++] = (board_id >> 16) & 0xFF;
    binary_data[offset++] = (board_id >> 8) & 0xFF;
    binary_data[offset++] = board_id & 0xFF;

    // Red, Green, and Blue RGB values (1 byte each)
    binary_data[offset++] = red_rgb;
    binary_data[offset++] = green_rgb;
    binary_data[offset++] = blue_rgb;

    // Status values
    binary_data[offset++] = led1_status;
    binary_data[offset++] = led2_status;
    binary_data[offset++] = relay_status;
    binary_data[offset++] = buzzer_status;
    binary_data[offset++] = buzzer_code;
    binary_data[offset++] = buzzer_timeout;

    // Reserved bytes
    memset(&binary_data[offset], 0, 5);  // 5 reserved bytes
    offset += 5;

    // Allocate memory for hex string
    char *hex_string = malloc(binary_length * 2 + 1);
    if (!hex_string) {
        perror("Memory allocation failed");
        free(binary_data);
        return NULL;
    }

    // Convert binary data to hex string
    for (size_t i = 0; i < binary_length; i++) {
        sprintf(hex_string + 2 * i, "%02X", binary_data[i]);
    }

    // Null-terminate the hex string
    hex_string[binary_length * 2] = '\0';

    free(binary_data);
    return hex_string;
}
