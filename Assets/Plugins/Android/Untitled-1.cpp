#include <iostream>
#include <vector>

using namespace std;

unsigned long long factorial(int n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
}

unsigned long long combinacion(int n, int r) {
    return factorial(n) / (factorial(r) * factorial(n - r));
}

unsigned long long permutacion(int n, int r) {
    return factorial(n) / factorial(n - r);
}

double probabilidadSimple(int casosFavorables, int totalCasos) {
    return static_cast<double>(casosFavorables) / totalCasos;
}

double probabilidadEventos(vector<double> probabilidades) {
    double resultado = 1.0;
    for (double p : probabilidades) {
        resultado *= p;
    }
    return resultado;
}

double probabilidadCondicional(double probA, double probAinterB) {
    return probAinterB / probA;
}

void menu() {
    cout << "=== Calculadora de Probabilidad ===" << endl;
    cout << "1. Probabilidad simple" << endl;
    cout << "2. Combinaciones y Permutaciones" << endl;
    cout << "3. Probabilidad de eventos independientes" << endl;
    cout << "4. Probabilidad condicional" << endl;
    cout << "5. Salir" << endl;
    cout << "Seleccione una opción: ";
}

int main() {
    int opcion;

    do {
        menu();
        cin >> opcion;

        switch (opcion) {
            case 1: {
                int casosFavorables, totalCasos;
                cout << "Ingrese el número de casos favorables: ";
                cin >> casosFavorables;
                cout << "Ingrese el total de casos posibles: ";
                cin >> totalCasos;
                if (totalCasos > 0) {
                    cout << "La probabilidad es: "
                         << probabilidadSimple(casosFavorables, totalCasos) << endl;
                } else {
                    cout << "El total de casos debe ser mayor que cero." << endl;
                }
                break;
            }

            case 2: {
                int n, r;
                cout << "Ingrese el valor de n (total de elementos): ";
                cin >> n;
                cout << "Ingrese el valor de r (elementos a tomar): ";
                cin >> r;

                if (n >= r) {
                    cout << "1. Combinaciones (nCr)" << endl;
                    cout << "2. Permutaciones (nPr)" << endl;
                    int opcionSub;
                    cin >> opcionSub;

                    if (opcionSub == 1) {
                        cout << "El resultado de la combinación es: " 
                             << combinacion(n, r) << endl;
                    } else if (opcionSub == 2) {
                        cout << "El resultado de la permutación es: "
                             << permutacion(n, r) << endl;
                    } else {
                        cout << "Opción inválida." << endl;
                    }
                } else {
                    cout << "n debe ser mayor o igual que r." << endl;
                }
                break;
            }

            case 3: {
                int n;
                cout << "Ingrese el número de eventos: ";
                cin >> n;

                vector<double> probabilidades(n);
                cout << "Ingrese las probabilidades de cada evento:" << endl;
                for (int i = 0; i < n; ++i) {
                    cout << "Evento " << i + 1 << ": ";
                    cin >> probabilidades[i];
                }

                cout << "La probabilidad conjunta es: "
                     << probabilidadEventos(probabilidades) << endl;
                break;
            }

            case 4: {
                double probA, probAinterB;
                cout << "Ingrese P(A): ";
                cin >> probA;
                cout << "Ingrese P(A ∩ B): ";
                cin >> probAinterB;

                if (probA > 0) {
                    cout << "La probabilidad condicional P(B|A) es: "
                         << probabilidadCondicional(probA, probAinterB) << endl;
                } else {
                    cout << "P(A) debe ser mayor que cero." << endl;
                }
                break;
            }

            case 5:
                cout << "Saliendo del programa." << endl;
                break;

            default:
                cout << "Opción inválida. Intente de nuevo." << endl;
                break;
        }

        cout << endl;

    } while (opcion != 5);

    return 0;
}