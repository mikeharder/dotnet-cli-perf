package ClassLib081;

public class Class001 {
    public static String property() {
        return "ClassLib081" + " " + ClassLib001.Class001.property() + " " + ClassLib070.Class001.property();
    }
}
